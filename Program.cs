using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class MatrixOperations
{
    private int[,] matrix1;
    private int[,] matrix2;
    private int[,] matrix3;

    // Конструктор для задания 1
    public MatrixOperations(int n, int m)
    {
        matrix1 = new int[n, m];
        Console.WriteLine("Введите элементы первого массива по столбцам:");
        for (int j = 0; j < m; j++)
        {
            for (int i = 0; i < n; i++)
            {
                matrix1[i, j] = int.Parse(Console.ReadLine());
            }
        }

        matrix2 = new int[n, n];
        Random rand = new Random();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                int num = rand.Next(100, 1000);
                string numStr = num.ToString();
                if (numStr[0] < numStr[1] && numStr[1] < numStr[2])
                {
                    matrix2[i, j] = num;
                }
                else
                {
                    j--;
                }
            }
        }

        matrix3 = new int[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matrix3[i, j] = rand.Next(1, 10);
            }
        }
    }

    // Метод для задания 2
    public int[,] CreateTwoDimensionalArray(int[] oneDimensionalArray)
    {
        int n = oneDimensionalArray.Length;
        int[,] resultMatrix = new int[n, n];

        for (int j = 0; j < n; j++)
        {
            int sum = oneDimensionalArray[j];
            for (int i = 0; i < n; i++)
            {
                resultMatrix[i, j] = 1; // Заполняем единицами
            }

            int currentSum = resultMatrix.Cast<int>().Take(n).Sum();
            int adjustment = sum - currentSum;

            if (adjustment > 0)
            {
                resultMatrix[n - 1, j] += adjustment; // Добавляем остаток
            }
        }

        return resultMatrix;
    }

    // Метод для задания 3
    public int[,] CalculateMatrixExpression(int[,] B, int[,] C)
    {
        int n = B.GetLength(0);
        int[,] result = new int[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                result[i, j] = matrix1[i, j] - (3 * B[i, j] - C[i, j]);
            }
        }

        return result;
    }

    public override string ToString()
    {
        return $"Matrix1:\n{MatrixToString(matrix1)}\nMatrix2:\n{MatrixToString(matrix2)}\nMatrix3:\n{MatrixToString(matrix3)}";
    }

    public static string MatrixToString(int[,] matrix)
    {
        var result = "";
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result += matrix[i, j] + "\t";
            }
            result += "\n";
        }
        return result;
    }
}

public class FileOperations
{
    // Метод для заполнения файла случайными числами
    public static void FillFileWithRandomNumbers(string filePath, int count, int numbersPerLine)
    {
        Random rand = new Random();
        using (var writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < count; i++)
            {
                if (i > 0 && i % numbersPerLine == 0)
                {
                    writer.WriteLine(); // Новая строка после каждого набора чисел
                }
                writer.Write(rand.Next(int.MinValue, int.MaxValue) + (i % numbersPerLine == numbersPerLine - 1 ? "\n" : " "));
            }
        }
    }

    // Метод для создания файла из исходного
    public static void CreateFileFromSource(string sourceFile, string destinationFile)
    {
        var lines = File.ReadAllLines(sourceFile);
        using (var writer = new StreamWriter(destinationFile))
        {
            for (int i = 1; i <= lines.Length; i++)
            {
                writer.WriteLine(string.Join(" ", lines.Take(i)));
            }
        }
    }

    public static (int max, int min) FindMaxMin(string filePath)
    {
        var numbers = File.ReadAllLines(filePath).Select(int.Parse).ToList();
        int max = numbers.Max();
        int min = numbers.Min();
        return (max, min);
    }

    public static void CountOddNumbers(string filePath)
    {
        var numbers = File.ReadAllLines(filePath)
                          .SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                          .Select(int.Parse));
        int oddCount = numbers.Count(n => n % 2 != 0);
        Console.WriteLine($"Количество нечётных элементов: {oddCount}");
    }

    // Метод для задания 8
    public static void CreateLineLengthFile(string sourceFile, string destinationFile)
    {
        var lines = File.ReadAllLines(sourceFile);
        using (var writer = new StreamWriter(destinationFile))
        {
            foreach (var line in lines)
            {
                writer.WriteLine(line.Length);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        MatrixOperations matrixOps = null;

        while (true)
        {
            Console.WriteLine("Выберите задание (1-8) или 0 для выхода:");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > 8)
            {
                Console.WriteLine("Ошибка: введите число от 0 до 8.");
                continue;
            }

            if (choice == 0)
                break; // Выход из программы

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Введите размеры матрицы (n m):");
                    var sizes = Console.ReadLine().Split();
                    if (sizes.Length != 2 || !int.TryParse(sizes[0], out int n1) || !int.TryParse(sizes[1], out int m1) || n1 <= 0 || m1 <= 0)
                    {
                        Console.WriteLine("Ошибка: введите два положительных целых числа.");
                        break;
                    }
                    matrixOps = new MatrixOperations(n1, m1);
                    Console.WriteLine(matrixOps.ToString());
                    break;

                case 2:
                    if (matrixOps == null)
                    {
                        Console.WriteLine("Сначала выполните задание 1 для инициализации матрицы.");
                        break;
                    }
                    Console.WriteLine("Введите элементы одномерного массива (через пробел):");
                    var inputArray = Console.ReadLine().Split();
                    int[] oneDimArray = new int[inputArray.Length];
                    bool isValidArray = true;

                    for (int i = 0; i < inputArray.Length; i++)
                    {
                        if (!int.TryParse(inputArray[i], out oneDimArray[i]))
                        {
                            isValidArray = false;
                            Console.WriteLine($"Ошибка: '{inputArray[i]}' не является целым числом.");
                            break;
                        }
                    }

                    if (isValidArray)
                    {
                        var resultMatrix = matrixOps.CreateTwoDimensionalArray(oneDimArray);
                        Console.WriteLine($"Результирующий двумерный массив:\n{MatrixOperations.MatrixToString(resultMatrix)}");
                    }
                    break;

                case 3:
                    if (matrixOps == null)
                    {
                        Console.WriteLine("Сначала выполните задание 1 для инициализации матрицы.");
                        break;
                    }
                    Console.WriteLine("Введите размерность матриц (n):");
                    if (!int.TryParse(Console.ReadLine(), out int n2) || n2 <= 0)
                    {
                        Console.WriteLine("Ошибка: введите положительное целое число.");
                        break;
                    }
                    int[,] B = new int[n2, n2];
                    int[,] C = new int[n2, n2];
                    Random rand = new Random();
                    for (int i = 0; i < n2; i++)
                        for (int j = 0; j < n2; j++)
                        {
                            B[i, j] = rand.Next(1, 10);
                            C[i, j] = rand.Next(1, 10);
                        }
                    var result = matrixOps.CalculateMatrixExpression(B, C);
                    Console.WriteLine($"Результат матричного выражения:\n{MatrixOperations.MatrixToString(result)}");
                    break;

                case 4:
                    FileOperations.CreateFileFromSource("string.txt", "destination.txt");
                    Console.WriteLine("Файл создан согласно заданию 4.");
                    break;

                case 6:
                    // Заполнение файла случайными числами и нахождение максимума и минимума
                    Console.WriteLine("Введите количество чисел для записи в файл (numbers.txt):");
                    if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
                    {
                        Console.WriteLine("Ошибка: введите положительное целое число.");
                        break;
                    }
                    FileOperations.FillFileWithRandomNumbers("numbers.txt", count, 1); // 1 число в строке
                    Console.WriteLine($"Файл 'numbers.txt' заполнен {count} случайными числами.");

                    // Поиск максимального и минимального значений
                    var (max, min) = FileOperations.FindMaxMin("numbers.txt");
                    Console.WriteLine($"Максимальное значение: {max}, Минимальное значение: {min}");
                    break;

                case 7:
                    // Заполнение файла случайными нечетными числами
                    Console.WriteLine("Введите количество случайных чисел для записи в файл (odd_numbers.txt):");
                    if (!int.TryParse(Console.ReadLine(), out int countForOdd) || countForOdd <= 0)
                    {
                        Console.WriteLine("Ошибка: введите положительное целое число.");
                        break;
                    }
                    FileOperations.FillFileWithRandomNumbers("odd_numbers.txt", countForOdd, 5); // 5 чисел в строке
                    Console.WriteLine($"Файл 'odd_numbers.txt' заполнен {countForOdd} случайными нечетными числами.");

                    // Подсчет нечетных чисел
                    FileOperations.CountOddNumbers("odd_numbers.txt");
                    break;

                case 8:
                    FileOperations.CreateLineLengthFile("string.txt", "lengths.txt");
                    Console.WriteLine("Файл с длинами строк создан.");
                    break;

                default:
                    Console.WriteLine("Некорректный выбор задания.");
                    break;
            }
        }
    }
}
