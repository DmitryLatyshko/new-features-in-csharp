#nullable enable

using System;

namespace NullableReferenceTypes
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

			WriteLineAndWaitPressEnter();

			ExampleThird();

			WriteLineAndWaitPressEnter();

			ExampleFourth();

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
			var person = new PersonFirst();
			person.name = null;
			person.nickname = null;
			var nameLen = person.name.Length;
			var nicknameLen = person.nickname.Length;

			/*
			 * Скомпилировав этот код, мы получим два предупреждения (жаль, что по умолчанию
			 * компилятор не считает подобный код ошибочным, но это легко исправить):
			 * [CS8618) Non-nullable field 'name' is uninitialized. Consider declaring the field as nullable.
			 * и [CS8625] Cannot convert null literal to non-nullable reference type.
			 * Другими словами, теперь все ссылочные типы по умолчанию не могут принимать значение null.
			 * Если необходимо выставить значение в null, нужно использовать знак вопроса после ссылочного типа.
			 */
			/*
			 * В отличии от Nullable Value Types, знак вопроса ни как не изменяет тип рядом с ним, т.е. в данном
			 * случае поле nickname будет иметь тип System.String, а не какой нибудь Nullable<System.String>.
			 * Таким образом, в данном случае единственное, что делает знак вопроса, это добавляет подсказку
			 * компилятору, что данное поле можно занулять. По факту, дополнительно добавляется соответствующий
			 * атрибут System.Runtime.CompilerServices.NullableAttribute.
			 */
		}

		#endregion

		#region ExampleSecond

		static void ExampleSecond()
		{
			/*
			 * Исправленный код (не генерирующий предупреждения компилятором).
			 */
			var person = new PersonSecond("TestName");
			var nameLength = person.name.Length;
			var nicknameLength = person.nickname != null ? person.nickname.Length : -1;
		}

		#endregion

		#region ExampleThird

		static void ExampleThird()
		{
			var person = GetPersonThird();
			AssertNotNullThird(person);
			Console.WriteLine(person.name);

			/*
			 * В строчке вывода мы однозначно не можем получить null из-за того, что строчкой выше.
			 * В таком случае будет сгенерировано исключение: компилятор (к сожалению, он не настолько умен,
			 * как хотелось бы) сгенерирует предупреждение: [CS8602] Dereference of a possibly null reference.
			 * Это, конечно же, печально. Но как же быть? Неужто теперь придется в каждом подобном (и куче других)
			 * случае добавлять явную (да еще и избыточную) проверку на null? Да, это один из вариантов решения
			 * (возможно даже, не самый худший).
			 */
		}

		static PersonThird? GetPersonThird()
		{
			return null;
		}

		static void AssertNotNullThird(PersonThird? value)
		{
			if (value == null) {
				throw new ArgumentNullException(nameof(value));
			}
		}

		#endregion

		#region ExampleFourth

		static void ExampleFourth()
		{
			/*
			 * Другим вариантом решения является новые Null-Forgiving оператор: !, который как бы говорит
			 * компилятору: "Не парься! Я знаю, что делаю" (ага, как же)).
			 */

			var person = GetPersonFourth();
			AssertNotNullFourth(person);
			Console.WriteLine(person!.name);

			/*
			 * И компилятор не будет генерировать исключение.
			 *
			 * Пока существует возможность выстрелить в ногу, ни о какой null-безопасности речи идти не может.
			 * Представьте, что в нашем примере по ходу развития программы урали метод AssertNotNull или проверку в нем,
			 * но на восклицательный знак, скорее всего, никто не обратит внимание и мы получим ошибку.
			 *
			 * Стоит еще раз отметить, что никаких изменений в типах или дополнительных проверок в run-time не происходит.
			 * Мы имеем примерно то же самое, что имели раньше, используя JetBrains Annotations & Rider/ReSharper,
			 * только из коробки.
			 * Глобальное Nullable reference types, к большому сожалению, проблему не решает.
			 */
		}

		static PersonFourth? GetPersonFourth()
		{
			return null;
		}

		static void AssertNotNullFourth(PersonFourth? value)
		{
			if (value == null) {
				throw new ArgumentNullException(nameof(value));
			}
		}

		#endregion
	}

	#region For ExampleFirst

	class PersonFirst
	{
		public string name;
		public string? nickname;
	}

	#endregion

	#region For ExampleSecond

	class PersonSecond
	{
		public string name;
		public string? nickname;

		public PersonSecond(string name, string? nickname = null)
		{
			this.name = name;
			this.nickname = nickname;
		}
	}

	#endregion

	#region For ExampleThird

	class PersonThird
	{
		public PersonThird()
		{
			this.name = "TestName";
		}

		public string? name;
	}

	#endregion

	#region For ExampleFourth

	public class PersonFourth
	{
		public PersonFourth()
		{
			this.name = "TestName";
		}

		public string? name;
	}

	#endregion
}
