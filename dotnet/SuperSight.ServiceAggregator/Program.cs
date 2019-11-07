using SuperSight.ServiceAggregator;
using System;
using System.Threading.Tasks;

namespace ServiceAggregator
{
    class Program
    {
        /// <summary>
        /// Launch tasks to process frames
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            using (var processorFrames = new FrameProcessorFrames())
            {
                using (var dogBreed = new FrameProcessorDogBreed())
                {
                    using (var ageGender = new FrameProcessorAgeGender())
                    {
                        try
                        {
                            var taskDogBreed = Task.Run(() => dogBreed.Process());
                            var taskAgeGender = Task.Run(() => ageGender.Process());
                            var taskFrameProcessor = Task.Run(() => processorFrames.Process(ageGender, dogBreed));

                            taskFrameProcessor.Wait();
                            taskDogBreed.Wait();
                            taskAgeGender.Wait();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
        }
    }
}
