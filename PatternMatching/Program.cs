using System;
using System.Drawing;

namespace PatternMatching
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

			/*
			 * Таким образом, сопоставление с образцом - крайне полезная возможность, которая позволяет
			 * довольно сильно сократить и упростить некогда сложный код, сделать его более читаемым и
			 * безопасным из-за полезных предупреждений от компилятора.
			 */

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
			var person = new Person("Ivan", "SuperMan", 22);
			var socialGroup = person switch
			{
				{age: 14} => "Teenager",
				{nickname: null} => "Pensioner",
				{age: 22} => "Worker"
			};

			Console.WriteLine($"{person.name} social group: {socialGroup}");

			/*
			 * В консоль будет выведена следующая строка: Ivan social group: Worker.
			 * Можно задаться вопросом: а что же произойдет, если ни какой из описанных вариантов не сработает?
			 * На ум приходит, как минимум, два варианта:
			 * 1. возможно, т.к. это выражение, то вернется default значение;
			 * 2. выброс исключения.
			 * Дизайнеры языка остановились на последнем варианте. И, в таком случае, будет выкинуто исключение:
			 * Unmatched value was Program+Person. Также стоит отметить, что, если скомпилировать вышеприведенный код,
			 * то будет сгенерировано следующее предупреждение:
			 * [CS8509] The switch expression does not handle all possible inputs (it is not exhaustive).,
			 * которое нам намекает, что мы обработали не все случаи. И этот момент разительно (в положительную сторону)
			 * отличается от поведения стандартного switch statement. Само собой, в данном случае мы не можем перечислить
			 * все возможные варианты, т.к. их неимоверно много. Здесь нам пригодилось бы ключевое слово default из
			 * стандартного свитча.
			 */

			/*
			 * Но, вместо него, как вы уже догадались, по аналогии с другими вариантами сопоставления
			 * с образцом используется нижнее подчеркивание '_'. Новое выражение можно использовать не только для обработки полей и
			 * свойств, но и в качестве более удобной и лаконичной замены стандартному switch statement
			 */

			#region Ext

			var rainbowColor = GetColor();
			var color = rainbowColor switch
			{
				Rainbow.Red => Color.Red,
				Rainbow.Orange => Color.Orange,
				Rainbow.Yellow => Color.Yellow,
				Rainbow.Green => Color.Green,
				Rainbow.Blue => Color.Blue,
				Rainbow.Indigo => Color.Indigo,
				Rainbow.Violet => Color.Violet,
				_ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(rainbowColor)),
			};

			/*
			 * Аналог привычным образом:
			 */
			switch (rainbowColor) {
				case Rainbow.Red: {
					color = Color.Red;
					break;
				}
				case Rainbow.Orange: {
					color = Color.Orange;
					break;
				}
				case Rainbow.Yellow: {
					color = Color.Yellow;
					break;
				}
				case Rainbow.Green: {
					color = Color.Green;
					break;
				}
				case Rainbow.Blue: {
					color = Color.Blue;
					break;
				}
				case Rainbow.Indigo: {
					color = Color.Indigo;
					break;
				}
				case Rainbow.Violet: {
					color = Color.Violet;
					break;
				}
				default: {
					throw new ArgumentException(message: "invalid enum value", paramName: nameof(rainbowColor));
				}
			}

			#endregion
		}

		static Rainbow GetColor() { return Rainbow.Blue; }

		#endregion

		#region ExampleSecond

		static void ExampleSecond()
		{
			/*
			 * Аналогично мы можем делать сопоставление с образцом кортежей
			 */

			var rock = RockPaperScissors("rock", "paper");
			Console.WriteLine(rock);

			/*
			 * Кроме этого, мы можем использовать извлеченные из обрабатываемых структур и
			 * накладывать на них органичения
			 */
			var point = new Point(1, -1);
			var quadrant = GetQuadrant(point);
			Console.WriteLine(quadrant);
		}

		static string RockPaperScissors(string first, string second) =>
			(first, second) switch
			{
				("rock", "paper") => "rock is covered by paper. Paper wins.",
				("rock", "scissors") => "rock breaks scissors. Rock wins.",
				("paper", "rock") => "paper covers rock. Paper wins.",
				("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
				("scissors", "rock") => "scissors is broken by rock. Rock wins.",
				("scissors", "paper") => "scissors cuts paper. Scissors wins.",
				(_, _) => "tie"
			};

		static Quadrant GetQuadrant(Point point) =>
			point switch
			{
				(0, 0) => Quadrant.Origin,
				var (x, y) when x > 0 && y > 0 => Quadrant.One,
				var (x, y) when x < 0 && y > 0 => Quadrant.Two,
				var (x, y) when x < 0 && y < 0 => Quadrant.Three,
				var (x, y) when x > 0 && y < 0 => Quadrant.Four,
				var (_, _) => Quadrant.OnBorder,
				_ => Quadrant.Unknown
			};

		#endregion
	}

	class Person
	{
		public string name;
		public string nickname;
		public int age;

		public Person(string name, string nickname, int age)
		{
			this.name = name;
			this.nickname = nickname;
			this.age = age;
		}
	}

	enum Rainbow
	{
		Red,
		Orange,
		Yellow,
		Green,
		Blue,
		Indigo,
		Violet
	}

	class Point
	{
		public int X { get; }

		public int Y { get; }

		public Point(int x, int y) => (X, Y) = (x, y);

		public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
	}

	enum Quadrant
	{
		Origin,
		OnBorder,
		Unknown,
		One,
		Two,
		Three,
		Four,
	}
}
