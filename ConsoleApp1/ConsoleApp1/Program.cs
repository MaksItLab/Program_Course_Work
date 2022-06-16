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
            Console.ReadLine();
        }
    }
    internal class Graph
    {
        private int _columns;
        private int _lines;
        private int _maxQ;
        private int _currentVertex;
        private bool _coolVertax = false;

        private List<List<List<int>>> _array = new List<List<List<int>>>();
        private List<List<int>> _arrayCopy = new List<List<int>>();
        private List<List<int>> _vertaxs = new List<List<int>>();
        private List<int> _path = new List<int>();
        private List<int> _coolPath = new List<int>();


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
                _arrayCopy.Add(new List<int>());
                string[] arrayString = arrayLines[i].Split(' ');
                for (int j = 0; j < arrayString.Length; j++)
                {
                    _array.Add(new List<List<int>>());
                    _array[i].Add(new List<int>() { int.Parse(arrayString[j]) }); //Заполнение листа из матрицы
                    _arrayCopy[i].Add( int.Parse(arrayString[j]) );
                }
            }
        }

        

        

        public void Algoritm(int source, int stock) //Алгоритм Франка-Фриша
        {
            _path.Add(source);
            _currentVertex = source;
            while (!_path.Contains(stock))
            {
                _maxQ = MaxQ(_currentVertex);
                for (int i = 0; i < _lines - 1; i++)
                {
                    for (int j = i + 1; j < _columns; j++)
                    {
                        for (int g = 0; g < _array[i][j].Count; g++)
                        {
                            if (_array[i][j][g] >= _maxQ)
                            {
                                DeleteEdge(i, j);
                                ClearVertaxs();
                                Console.WriteLine("---------------------------------------------------------");
                                PrintArray();
                                if (_path.Contains(stock))
                                {
                                    FindPath(source, stock, source);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("---------------------------------------------------------");
                ClearVertaxs();
                PrintArray();
            }
        }
        //public void FindPath(int source, int stock, int parent)
        //{
        //    for (int i = 0; i < _lines; i++)
        //    {
        //        for (int j = 0; j < _columns; j++)
        //        {
        //            if (_arrayCopy[i][j] >= _maxQ)
        //            {
        //                _coolPath.Add(j);
        //                i = j;
        //                j = 0;
        //            }
        //            if (_arrayCopy[i].Contains(-1))
        //            {

        //            }
        //        }
        //    }
        //}

        public bool FindPath(int source, int stock, int parent)
        {
            if (source == stock)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < _lines; i++)
                {
                    if (_arrayCopy[source][i] >= _maxQ && i != parent)
                    {
                        if (FindPath(i, stock, source))
                        {
                            _coolPath.Add(i);
                        }
                        
                    }
                }
                return false;
            }
        }



        public void RibShortening(int vertax_line, int vertax_column)
        {
            //для 2х вершин
            //удаляем ребро
            //добавляем вершину в лист другой вершины
            //удаляем лист со старой вершиной
            //копируем пути из старой вершины в новую
            //присваиваем нули путям старой вершине

            Console.WriteLine("_______________________________________");
            PrintArray();
            
            
            for (int i = vertax_line; i < vertax_line + 1; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    for (int g = 0; g < _array[vertax_column][j].Count; g++)
                    {
                        if (_array[vertax_column][j][g] != -1 && _array[vertax_column][j][g] != 0 && _array[vertax_column][j][g] < _maxQ)
                        {
                            if (_array[vertax_line][j].Contains(-1))
                            {
                                _array[vertax_column][j][g] = 0;
                            }
                            else
                            {
                                _array[i][j].Add(_array[vertax_column][j][g]);
                                _array[j][i].Add(_array[vertax_column][j][g]);
                                _array[vertax_column][j][g] = 0;
                                _array[j][vertax_column][g] = 0;
                            }
                        }
                    }
                }
            }
            Console.WriteLine("_______________________________________");
            PrintArray();

        }

        public void ClearVertaxs()
        {
            for (int i = 0; i < _lines; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    for (int g = 0; g < _array[i][j].Count; g++)
                    {
                        if (_array[i][j].Count > 1 && _array[i][j][g] == 0)
                        {
                            _array[i][j].Remove(g);
                        }
                    }
                }
            }
        }

        public void DeleteEdge(int vertax_line, int vertax_column)
        {
            _array[vertax_line].RemoveAt(vertax_column);
            _array[vertax_column].RemoveAt(vertax_line);
            _array[vertax_line].Insert(vertax_column, new List<int> { -1 });
            _array[vertax_column].Insert(vertax_line, new List<int> { -1 });

            if (_vertaxs[vertax_line][0] == -1)
            {
                for (int i = 0; i < _vertaxs.Count; i++)
                {
                    if (_vertaxs[i].Contains(vertax_line))
                    {
                        for (int j = 0; j < _vertaxs[vertax_column].Count; j++)
                        {
                            _vertaxs[i].Add(_vertaxs[vertax_column][j]);
                            if (vertax_line == 0 && !_path.Contains(_vertaxs[vertax_column][j]))
                            {
                                _path.Add(_vertaxs[vertax_column][j]);
                            }
                        }
                        vertax_line = i;
                    }
                }
            }
            else if(_vertaxs[vertax_column][0] == -1)
            {
                for (int i = 0; i < _vertaxs.Count; i++)
                {
                    if (_vertaxs[i].Contains(vertax_column))
                    {
                        for (int j = 0; j < _vertaxs[vertax_line].Count; j++)
                        {
                            _vertaxs[i].Add(_vertaxs[vertax_line][j]);
                            if (vertax_line == 0 && !_path.Contains(_vertaxs[vertax_column][j]))
                            {
                                _path.Add(_vertaxs[vertax_column][j]);
                            }
                        }
                        vertax_column = vertax_line;
                        vertax_line = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _vertaxs[vertax_column].Count; i++)
                {
                    _vertaxs[vertax_line].Add(_vertaxs[vertax_column][i]);
                    if (vertax_line == 0 && !_path.Contains(_vertaxs[vertax_column][i]))
                    {
                        _path.Add(_vertaxs[vertax_column][i]);
                    }
                }
            }
            
            
            _vertaxs.RemoveAt(vertax_column);
            _vertaxs.Insert(vertax_column, new List<int> { -1 });

            RibShortening(vertax_line, vertax_column);
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

        public void PrintArray() //Печать массива
        {
            for (int i = 0; i < _lines; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    for (int g = 0; g < _array[i][j].Count; g++)
                    {
                        if (g == _array[i][j].Count - 1)
                        {
                            Console.Write(_array[i][j][g] + " ");
                        }
                        else
                        {
                            Console.Write(_array[i][j][g] + ",");
                        }
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}


