using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.Await
{
    public class TestAwait : MonoBehaviour
    {
        List<int> numbers = new List<int>();
        
        private void Start()
        {
            TestAddNumber();
        }

        async void TestAddNumber()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(AddNumberFrom(1, 10));
            tasks.Add(AddNumberFrom(20, 40));
            tasks.Add(RemoveNumberFrom(0, 3));
            await Task.WhenAll(tasks);
            Debug.Log("TestAddNumber completed");
        }
        
        async Task RemoveNumberFrom(int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                Debug.Log($"RemoveNumberFrom: {i}");
                lock (numbers)
                {
                    numbers.Remove(i);
                }
            }
        }

        async Task AddNumberFrom(int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                Debug.Log($"AddNumberFrom: {i}");
                lock (numbers)
                {
                    numbers.Add(i);
                }
            }
        }
        
        async void TestAllTaskCompleted()
        {
            Debug.Log("TestAllTaskCompleted.Start");
            List<Task> tasks = new List<Task>();
            tasks.Add(AwaitExample1());
            tasks.Add(AwaitExample2());
            
            await Task.WhenAll(tasks);
            
            Debug.Log("TestAllTaskCompleted completed");
        }

        async void TestAwaiter()
        {
            Debug.Log("TestAwait.Start");
            AwaitExample1().GetAwaiter();
            
            Debug.Log("TestAwait.Start Main Thread");

            await AwaitExample2();
            
            Debug.Log("TestAwait.Start completed");
        }

        private async Task AwaitExample2()
        {
            Debug.Log("AwaitExample2 started");
            await Task.Delay(1000); 
            Debug.Log("AwaitExample completed after 1 seconds");
        }
        
        private async Task AwaitExample1() 
        {
            Debug.Log("AwaitExample1 started");
            await Task.Delay(2000); 
            Debug.Log("AwaitExample completed after 2 seconds");
        }
    }
}