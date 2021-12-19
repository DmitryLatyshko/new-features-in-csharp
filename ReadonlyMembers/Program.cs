using System;

namespace ReadonlyMembers
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start!");
			Console.WriteLine();

			ExampleFirst();

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
			var person = new PersonFirst("Ivan", "Ivanov");
			Console.WriteLine(person.Dump());
		}

		#endregion
	}

	#region For ExampleFirst

	struct PersonFirst
	{
		public string name;
		public string surname;

		public PersonFirst(string name, string surname)
		{
			this.name = name;
			this.surname = surname;
		}

		public readonly string Dump() => $"Person({name}, {surname})";

		#region Case1

		/*
		 * При этом, если в таком метоже попробовать изменить данные, то мы получим ошибку компиляции:
		 * [CS1604] Cannot assign to 'variable' because it is read-only.
		 */
		// public readonly string DumpEdit() => $"Person({name = "NewName"}, {surname})";

		#endregion

		#region Case2

		/*
		 * При этом, если мы добавим обращение к геттеру без модификатора readonly, то мы получим уже предупреждение:
		 * [CS8656] Call to non-readonly property 'fullName' from a 'readonly' member results in an implicit copy of 'this'.
		 * И это - несмотря на то, что геттер, в данном случае, ни как не изменяет данные. Т.е. в таком случае
		 * будет создана копия структуры и вызов геттера будет с использованием копии.
		 * Это - потенциально узкое место по производительности. И, видимо, поэтому (т.к. в итоге оригинальная структура не изменяется)
		 * генерируется предупреждение, а не ошибка, как в предыдущем случае. Чтобы избавится от этого предупреждения,
		 * необходимо добавить модификатор readonly. Таким образом, модификатор только на чтение необходимо указывать
		 * и в свойствах только на чтение (геттерах). В случае авто-реализуемых свойств (auto-implemented properties), указывать
		 * модификатор не нужно, т.к. компилятор будет считать соответствующий геттер не изменяющим данные.
		 */
		public /*readonly*/ string fullName => $"{this.name}, {this.surname}";

		public readonly string DumpEdit2() => $"Person({fullName}";

		#endregion

	}

	#endregion
}
