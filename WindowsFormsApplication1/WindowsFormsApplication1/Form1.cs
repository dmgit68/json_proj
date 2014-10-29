using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        public class Worker
        {
            private BlockingQueue<String> m_queue;

            public Worker(BlockingQueue<String> aQueue)
            {
                m_queue = aQueue; 
            }

            // This method will be called when the thread is started. 
            public void DoWork(String aStr)
            {
                while (!_shouldStop)
                {
                    Console.WriteLine("worker thread: waiting...");
                    object obj = m_queue.Dequeue();

                    String myStr = obj as String;
                    Console.WriteLine(myStr);
                    
                }
                Console.WriteLine("worker thread: terminating gracefully.");
            }
            public void RequestStop()
            {
                _shouldStop = true;
            }
            // Volatile is used as hint to the compiler that this data 
            // member will be accessed by multiple threads. 
            private volatile bool _shouldStop;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            BlockingQueue<String> queue = new BlockingQueue<String>();

            Worker workerObject = new Worker(queue);
            //Thread workerThread = new Thread(workerObject.DoWork());
           // new ThreadStart(workerObject.DoWork("hello")) => download(filename));
            Thread workerThread = new Thread(() => workerObject.DoWork("hello"));
            // Start the worker thread.
            workerThread.Start();
            Console.WriteLine("main thread: Starting worker thread...");

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive);


            String myStr = "1";

            queue.Enqueue(myStr);

           // Thread.Sleep(1000);

            queue.Enqueue("2");


            //Thread.Sleep(10000);
            workerObject.RequestStop();

           // queue.Enqueue(10);
            workerThread.Join();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextWriter tw = File.CreateText(@"output.txt");
            tw.WriteLine("hello");
            tw.Flush();
            tw.WriteLine("world");
           // tw.Close();
        }
    }
}
