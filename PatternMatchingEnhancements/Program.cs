using System;

namespace PatternMatchingEnhancements
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
			Student nullCandidate = null;
			var candidate = new Student()
			{
				name = "Tom",
				surname = "Train",
				department = "South",
				privilege = false,
				score = 4.6,
			};
			Console.WriteLine(IsPasses(nullCandidate)); // false
			Console.WriteLine(IsPasses(candidate)); // true

			Console.WriteLine(IsPasses2(nullCandidate)); // false
			Console.WriteLine(IsPasses2(candidate)); // true
		}

		static bool IsPasses(Student student) { return student is ({ score: >= 4.4, } or { privilege: true }) and not { department: "Central" }; }

		/*
		 * Разумеется, всё это - синтаксический сахар. Тот же метод можно было бы реализовать,
		 * используя более классический синтаксис:
		 */

		#region Classic

		static bool IsPasses2(Student student) { return student != null && (student.score >= 4.4 || student.privilege == true) && student.department != "Central"; }

		/*
		 * Кстати, заметье, что в новом варианте проверка на null происходит автоматически.
		 * Да и, сравнивая методы между собой, несложно заметить, что новый синтаксис намного более читабельный.
		 */

		#endregion

		#endregion

		#region ExampleSecond

		static void ExampleSecond()
		{
			/*
			 * Что не менее важно, улучшенное сопоставление шаблонов костнулось и switch выражений.
			 */

			Student nullCandidate = null;
			var candidate = new Student()
			{
				name = "Tom",
				surname = "Train",
				department = "South",
				privilege = false,
				score = 4.2,
			};
			Console.WriteLine(IsPassesCommon(nullCandidate)); // false
			Console.WriteLine(IsPassesCommon(candidate)); // true
		}

		static bool IsPassesCommon(Student student) =>
			student switch
			{
				{ privilege: true } => true,
				{ score: >= 3.5 } and {score: <= 4.5 } => true,
				_ => false
			};

		#endregion
	}

	#region For ExampleFirst

	class Student
	{
		public string name;
		public string surname;
		public string department;
		public bool privilege;
		public double score;
	}

	#endregion

}
