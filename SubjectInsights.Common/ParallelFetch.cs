using SubjectInsights.Logger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SubjectInsights.Common
{
    public static class ParallelFetch
    {
        public static async Task Execute<U>(IEnumerable<U> dataList, Action<U> targetAction, int numberOfThreads)
        {
            try
            {
                var processAction = new ActionBlock<U>(item =>
                {
                    targetAction(item);
                }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = numberOfThreads });

                foreach (var item in dataList) await processAction.SendAsync(item);
                processAction.Complete();

                await processAction.Completion;
            }
            catch (Exception ex)
            {
                CommonLogger.LogError($"Error happened when Execute in parallel: {targetAction} | {ex.Message} {ex.StackTrace}");
            }
        }
        public static async Task BatchExecute<U>(IEnumerable<U> dataList, Action<IEnumerable<U>> targetAction, int numberOfThreads, int batchSize)
        {
            try
            {
                var batch = new BatchBlock<U>(batchSize);
                var action = new ActionBlock<IEnumerable<U>>(
                    targetAction,
                    new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = numberOfThreads });

                // link action to batch
                batch.LinkTo(action);

                // when batch complete, trigger action complete
                var continuation = batch.Completion.ContinueWith(_ => action.Complete());

                // begin sending data to batch block
                foreach (var data in dataList) await batch.SendAsync(data);
                batch.Complete();

                // wait until action complete
                await action.Completion;
            }
            catch (Exception ex)
            {
                CommonLogger.LogError($"Error happened when BatchExecute in parallel: {targetAction} | {ex.Message} {ex.StackTrace}");
            }
        }
        public static async Task ExecuteAsync<U>(IEnumerable<U> dataList, Func<U, Task> targetAction, int numberOfThreads)
        {
            try
            {
                var processAction = new ActionBlock<U>(async item =>
                {
                    await targetAction(item);
                }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = numberOfThreads });

                foreach (var item in dataList) await processAction.SendAsync(item);
                processAction.Complete();

                await processAction.Completion;
            }
            catch (Exception ex)
            {
                CommonLogger.LogError($"Error happened when ExecuteAsync in parallel: {targetAction} | {ex.Message} {ex.StackTrace}");
            }
        }
        public static async Task BatchExecuteAsync<U>(IEnumerable<U> dataList, Func<IEnumerable<U>, Task> targetAction, int numberOfThreads, int batchSize)
        {
            try
            {
                var batch = new BatchBlock<U>(batchSize);
                var action = new ActionBlock<IEnumerable<U>>(
                    targetAction,
                    new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = numberOfThreads });

                // link action to batch
                batch.LinkTo(action);

                // when batch complete, trigger action complete
                var continuation = batch.Completion.ContinueWith(_ => action.Complete());

                // begin sending data to batch block
                foreach (var data in dataList) await batch.SendAsync(data);
                batch.Complete();

                // wait until action complete
                await action.Completion;
            }
            catch (Exception ex)
            {
                CommonLogger.LogError($"Error happened when BatchExecuteAsync in parallel: {targetAction} | {ex.Message} {ex.StackTrace}");
            }
        }
        public static async Task<List<T>> GetBatchData<T, U>(List<U> dataList, Func<List<U>, Task<IEnumerable<T>>> targetAction, int numberOfThreads, int batchSize)
        {
            try
            {
                ConcurrentBag<IEnumerable<T>> combinedResult = new ConcurrentBag<IEnumerable<T>>();

                var processAction = new ActionBlock<List<U>>(async items =>
                {
                    var result = await targetAction(items);

                    combinedResult.Add(result);
                }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = numberOfThreads });

                for (int i = 0; i < dataList.Count; i = i + batchSize)
                {
                    await processAction.SendAsync(dataList.Skip(i).Take(batchSize).ToList());
                }

                processAction.Complete();
                await processAction.Completion;

                return combinedResult.SelectMany(mp => mp).ToList();
            }
            catch (Exception ex)
            {
                CommonLogger.LogError($"Error fetching parallel data, ParallelFetch.GetData: {targetAction} | {ex.Message} {ex.StackTrace}");
            }
            return new List<T>();
        }

    }
}
