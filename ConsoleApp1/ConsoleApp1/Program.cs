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
            graph.PrintArray();
            Console.ReadLine();
        }
    }
    internal class Graph
    {
        private int _columns;
        private int _lines;
        private int _maxQ;
        private int _currentVertex;

        private List<List<List<int>>> _array = new List<List<List<int>>>();
        private List<List<int>> _graph = new List<List<int>>();
        private List<List<int>> _vertaxs = new List<List<int>>();
        private List<int> _path = new List<int>();
        private List<int> _connectedVertices = new List<int>();
        


        public List<List<List<int>>> Array
        {
            get { return this._array; }

        }


        public Graph(string filename)
        {
            if (!File.Exists(filename)) //Проверка на наличие файла
            {
                throw new FileNotFoundException();
            }

            string[] arrayLines = File.ReadAllLines($"{filename}"); //Считывание матрицы из файла
            for (int i = 0; i < arrayLines.Length; i++)
            {
                _vertaxs.Add(new List<int> { i });
            }
            _lines = arrayLines.Length;
            _columns = arrayLines[0].Split(' ').Length;
            for (int i = 0; i < arrayLines.Length; i++)
            {
                string[] arrayString = arrayLines[i].Split(' ');
                _array.Add(new List<List<int>>());
                for (int j = 0; j < arrayString.Length; j++)
                {
                    int prob = int.Parse(arrayString[j]);
                    _array[i][j].Add(new List<int> { prob });//Заполнение листа из матрицы
                }
            }
        }

        public void PrintArray() //Печать массива
        {
            for (int i = 0; i < _lines; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    Console.Write(_array[i][j] + " ");
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
                        for (int g = 0; g < _array[i][j].Count; g++)
                        {
                            if (_array[i][j][g] >= _maxQ)
                            {
                                RibShortening(i, j, _maxQ);
                            }

                        }
                    }
                }
            }
        }

        public void RibShortening(int vertax_line, int vertax_column, int maximum)
        {
            //для 2х вершин
            //удаляем ребро
            //добавляем вершину в лист другой вершины
            //удаляем лист со старой вершиной
            //копируем пути из старой вершины в новую
            //присваиваем нули путям старой вершине
            

            //_connectedVertices.Add(vertax_line);
            //_connectedVertices.Add(vertax_column);
            _vertaxs[vertax_line].Append(vertax_column);
            _vertaxs.RemoveAt(vertax_column);
            _array[vertax_line].RemoveAt(vertax_column);
            _array[vertax_column].RemoveAt(vertax_line);
            _array[vertax_line].Insert(vertax_column, new List<int> { -1 });
            _array[vertax_column].Insert(vertax_line, new List<int> { -1 });
        }

        public int MaxQ(int vertex)
        {
            int max = _array[vertex][0][0];
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _array[vertex][i].Count; j++)
                {
                    if (_array[vertex][i][j] > max)
                    {
                        max = _array[vertex][i][j];
                    }
                }
                
            }
            return max;
        }
    }
}
