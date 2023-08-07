using Amazon.Runtime;
using SubjectInsights.Common.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Amazon.S3;
using System.Threading.Tasks;
using Amazon.S3.Transfer;
using SubjectInsights.Logger;
using Amazon.S3.Model;

namespace SubjectInsights.Common
{
    public static class S3Helper
    {
        public static async Task<string> UploadFileToS3(string base64content, string filename, IConfiguration config)
        {
            try
            {
                if (string.IsNullOrEmpty(base64content))
                {
                    return string.Empty;
                }
                byte[] bytes = Convert.FromBase64String(base64content);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    string uploadfilename = $"{Guid.NewGuid()}-{filename}";


                    var s3Obj = new ViewModel.S3Object()
                    {
                        BucketName = config["S3Details:BucketName"],
                        InputStream = ms,
                        Name = uploadfilename
                    };

                    var cred = new AwsCredentials()
                    {
                        AwsKey = config["S3Details:AWSAccessKey"],
                        AwsSecretKey = config["S3Details:AWSSecretKey"]
                    };

                    var result = await UploadFileAsync(s3Obj, cred);
                    if (result.StatusCode == 200)
                    {
                        return $"{config["S3Details:DocURL"]}/{uploadfilename}";
                    }
                    else
                    {
                        throw new Exception(result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return string.Empty;
            }

        }

        private static async Task<S3ResponseDto> UploadFileAsync(ViewModel.S3Object s3obj, AwsCredentials awsCredentials)
        {
            // Adding AWS credentials
            var credentials = new BasicAWSCredentials(awsCredentials.AwsKey, awsCredentials.AwsSecretKey);

            // Specify the region
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSouth1
            };

            var response = new S3ResponseDto();

            try
            {
                // Create the upload request
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = s3obj.InputStream,
                    Key = s3obj.Name,
                    BucketName = s3obj.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                // Created an S3 client
                using var client = new AmazonS3Client(credentials, config);

                // upload utility to s3
                var transferUtiltiy = new TransferUtility(client);

                // We are actually uploading the file to S3
                await transferUtiltiy.UploadAsync(uploadRequest);

                response.StatusCode = 200;
                response.Message = $"{s3obj.Name} has been uploaded successfully";
            }
            catch (AmazonS3Exception ex)
            {
                response.StatusCode = (int)ex.StatusCode;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        public static string GeneratePresignedURL(double duration, IConfiguration config)
        {
            var s3Obj = new ViewModel.S3Object()
            {
                BucketName = config["S3Details:BucketName"],
                Name = $"{Guid.NewGuid()}"
            };

            var cred = new AwsCredentials()
            {
                AwsKey = config["S3Details:AWSAccessKey"],
                AwsSecretKey = config["S3Details:AWSSecretKey"]
            };
            string urlString = string.Empty;
            var credentials = new BasicAWSCredentials(cred.AwsKey, cred.AwsSecretKey);

            // Specify the region
            var AWSconfig = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSouth1
            };
            using var client = new AmazonS3Client(credentials, AWSconfig);
            try
            {
                var request = new GetPreSignedUrlRequest()
                {
                    BucketName = s3Obj.BucketName,
                    Key = s3Obj.Name,
                    Expires = DateTime.UtcNow.AddMinutes(duration),
                };
                urlString = client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error:'{ex.Message}'");
            }

            return urlString;
        }
    }
}
