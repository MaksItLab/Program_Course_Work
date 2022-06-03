using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    internal class Program
    {
        public const string FILENAME = "data.txt";
        static void Main(string[] args)
        {
            Graph graph = new Graph(FILENAME);
            graph.PrintArray();
            Console.ReadLine();
            graph.Algoritm(0, 13);
            Console.WriteLine("проверка");
        }
    }
    internal class Graph
    {
        private int[,] _array;
        private int _columns;
        private int _lines;
        private List<List<int>> _graph;
        private List<int> _path;
        private List<int> _connectedVertices;
        private int _maxQ;
        private int _currentVertex;


        public int[,] Array
        {
            get { return this._array; }

        }


        public Graph(string filename)
        {
            if (!File.Exists(filename)) //Проверка на наличие файла
            {
                throw new FileNotFoundException();
            }

            string[] arrayLines = File.ReadAllLines($"{ filename}"); //Считывание матрицы из файла
            _lines = arrayLines.Length;
            _columns = arrayLines[0].Split(' ').Length;
            _array = new int[_lines, _columns];
            for (int i = 0; i < arrayLines.Length; i++)
            {
                string[] arrayString = arrayLines[i].Split(' ');
                for (int j = 0; j < arrayString.Length; j++)
                {
                    _array[i, j] = int.Parse(arrayString[j]); //Заполнение массива из матрицы
                }
            }
        }

        public void PrintArray() //Печать массива
        {
            for (int i = 0; i < _lines; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    Console.Write(_array[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void Algoritm(int sourse, int stock) //Алгоритм Франка-Фриша
        {
            _path.Add(sourse);
            _currentVertex = sourse;
            while (!_path.Contains(stock))
            {
                _maxQ = MaxQ(_currentVertex);
                for (int i = 0; i < _lines; i++)
                {
                    for (int j = 0; j < _columns; j++)
                    {
                        if (_array[i, j] >= _maxQ)
                        {
                            RibShortening(i, j);
                        }
                    }
                }


            }
        }

        public void RibShortening(int vertax_line, int vertax_column)
        {
            _connectedVertices.Add(vertax_line);
            _connectedVertices.Add(vertax_column);
            _array[vertax_line, vertax_column] = 0;
            _array[vertax_column, vertax_line] = 0;
        }

        public int MaxQ(int vertex)
        {
            int max = _array[vertex, 0];
            for (int i = 0; i < _columns; i++)
            {
                if (_array[vertex, i] > max)
                {
                    max = _array[vertex, i];
                }
            }
            return max;
        }
    }
}
