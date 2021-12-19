using System;
using System.Collections.Generic;

namespace IndicesAndRanges
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start!");
			Console.WriteLine();

			ExampleFirst();

			WriteLineAndWaitPressEnter();

			ExampleSecond();

			Console.WriteLine();
			Console.WriteLine("Done!");
		}

		static void WriteLineAndWaitPressEnter()
		{
			Console.WriteLine();
			Console.Write("Press 'Enter' to continue...");
			Console.ReadLine();
			Console.WriteLine();
		}

		#region ExampleFirst

		static void ExampleFirst()
		{
			Index myIndex = 0;
			Index anotherIndex = new Index(1, false);
			Index indexFromEnd = new Index(2, true);
			Index anotherIndexFromEnd = ^1;

			var array = new[] {1, 2, 3, 4};
			var list = new List<int> {1, 2, 3, 4};
			var @string = "1234";

			WriteFirst(array, list, @string, myIndex);
			WriteFirst(array, list, @string, anotherIndex);
			WriteFirst(array, list, @string, indexFromEnd);
			WriteFirst(array, list, @string, anotherIndexFromEnd);

			/*
			 * Стоит обратить внимание, что нумерация с конца начинается с 1! Т.к. запись list[^0]
			 * аналогична записи list[list.length]. Другими словами, в стандартных коллекциях при
			 * обращении по индексу ^0 мы получим исключение. В своих же коллекциях мы можем обработать и
			 * этот случай, и вернуть, возможно, поллезные данные.
			 */
		}

		static void WriteFirst(int[] array, List<int> list, string @string, Index index)
		{
			var strIndex = index.ToString().PadLeft(2);
			Console.WriteLine($"array[{strIndex}] = {array[index]}; list[{strIndex}] = {list[index]}; string[{index}] = {@string[index]}");
		}

		#endregion

		#region ExampleSecond

		static void ExampleSecond()
		{
			/*
			 * Диапазон Range представляет из себя линейный, сторого возврастающий диапазон индексов с шагом 1.
			 * При это старт диапазона включается, а конец исключается.
			 */

			var array = new[] {1, 2, 3, 4};

			WriteSecond(array, 0..4);
			WriteSecond(array, 0..);
			WriteSecond(array, ..^0);
			WriteSecond(array, 0..^0);
			WriteSecond(array, ..);
			WriteSecond(array, ^2..^0);
			WriteSecond(array, 0..3);
			WriteSecond(array, ..3);

			/*
			 * Вроде бы, все очевидно. Но есть и печаль, как уже выше было сказано: Диапазон должен быть
			 * строго возврастающим, т.е. если попробовать получить диапазон array[3..0], то мы получим:
			 * System.ArgumentOutOfRangeException: Specified argument was out of the range of valid values.
			 * По аналогии с предыдущим типом, здесь также можно учесть такие диапазоны в своих коллекциях.
			 * Т.е. всегда необходимо проверять, возрастающий ли диапазон или нет, при обращении к стандартным
			 * коллекциям (еще одно потенциальное слабое место). Другой неприятный момент - это шаг. Нельзя указать шаг,
			 * отличный от единицы, что также печалит, т.к. в том же F# мы можем без проблем указать шаг.
			 */
		}

		static void WriteSecond(int[] array, Range range)
		{
			var rangeStr = range.Start.ToString().PadLeft(2) + ".." + range.End.ToString().PadLeft(2);
			Console.WriteLine($"list[{rangeStr}] = {string.Join(", ", array[range])}");
		}

		#endregion
	}
}
