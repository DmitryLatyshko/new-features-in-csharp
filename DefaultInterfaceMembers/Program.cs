using System;

namespace DefaultInterfaceMembers
{
	public interface IUser
	{
	}

	public class User : IUser
	{
	}

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

		#region Простое использование реализованного метода в интерфейсе.

		static void ExampleFirst()
		{
			/*
			 * Простое использование реализованного метода в интерфейсе.
			 */
			var provider = new UserProvider();

			#region Methods: Calls

			GetUserByClass(provider);
			GetUserByInterface(provider);

			#endregion
		}

		#region Methods: Implementations

		static void GetUserByClass(UserProvider provider)
		{
			/*
			 * Compile error
			 *
			 * В данном случае стоит обратить внимание на то,
			 * что интерфейс IUserProvider содержит метод GetUser с реализацией и
			 * несмотря на то, что наш класс UserProvider реализует интерфейс  IUserProvider,
			 * явно реализовывать метод GetUser нет необходимости,
			 * так как интерфейс содержит реализацию по умолчанию.
			 *
			 * Можно подумать, что приведенный код скомпилируется и будет работать.
			 * Но это не так. Приведенный фрагмент кода генерирует следующую ошибку:
			 * [CS1061] 'UserProvider' does not contain a definition for 'GetUser' and
			 * no accessible extension method 'GetUser' accepting a first argument of type 'UserProvider' could be found
			 * (are you missing a using directive or an assembly reference?).
			 *
			 * Да как же так?
			 * Дело в том, что, как уже выше говорилось, основной сценарий использования этого
			 * нововведения — добавление методов в уже существующие интерфейсы.
			 *
			 * В таком случае получается, что если бы в классе  UserProvider уже был метод  GetUser,
			 * то произошел бы конфликт.
			 */
			Console.Write("provider.GetUser(); - ");
			// provider.GetUser();
			Console.WriteLine("Compile error!!!");
		}

		static void GetUserByInterface(UserProvider provider)
		{
			/*
			 * Compile success
			 *
			 * Чтобы обратится к реализованному в интерфейсе методу,
			 * необходимо явно откастить к интерфейсу.
			 */
			Console.Write("((IUserProvider) provider).GetUser(); - ");
			((IUserProvider) provider).GetUser();
		}

		#endregion

		#endregion

		#region Использование с наследованием...

		static void ExampleSecond()
		{
			/*
			 * Использование с наследованием...
			 *
			 * А что же будет, если у нас появится еще один интерфейс,
			 * который будет наследоваться от IUserProvider и также иметь
			 * метод GetUser с реализацией по умолчанию?
			 */
			var provider = new UserRemoteProvider();
			/*
			 * Вопрос к зрителям: Так код вообще скомпилируется?
			 *
			 * Да, такой код скомпилируется.
			 *
			 * А что же будет выведено в консоль при вызове метода GetUser() ?
			 * Если один интерфейс наследуется от другого, то можно предположить,
			 * что произойдет переопределение метода, в результате чего в консоль попадет две одинаковых строки:
			 * `IRemoteUserProvider.GetUser()`.
			 */

			#region Methods: Calls

			/*
			 * Что всё же мы увидим в консоли?
			 *
			 * На самом деле здесь ничего неожиданного быть не должно. Поведение точно такое же,
			 * как и в подобных случаях при определении перекрывающих методов в обычных класса.
			 */

			GetUserByBaseInterface(provider);
			GetUserByHeadInterface(provider);

			#endregion
		}

		#region Methods: Implementations

		static void GetUserByBaseInterface(UserRemoteProvider provider)
		{
			Console.Write("((IUserProvider) provider).GetUser(); - ");
			((IUserProvider) provider).GetUser();
		}

		static void GetUserByHeadInterface(UserRemoteProvider provider)
		{
			Console.Write("((IRemoteUserProvider) provider).GetUser(); - ");
			((IRemoteUserProvider) provider).GetUser();
		}

		#endregion

		#endregion

		#region Использование перекрытия при наследовании

		static void ExampleThird()
		{
			/*
			 * Использование перекрытия при наследовании
			 *
			 * В предыдущем примере мы уведели наследование, и два разных сообщения в консоли.
			 * Но это поведение, в данном случае, довольно легко исправить.
			 * Достаточно объявить метод UserProvider.GetUser() виртуальным и
			 * с помощью ключевого слова override переопределить этот метод в RemoteUserProvider.
			 *
			 * А доступна ли такая же возможность (полиморфизм) для методов интерфейса по умолчанию?
			 * И если доступна, то как ей воспользоваться?
			 * Возможно, стоит поступить аналогично примеру с классами?
			 */
			var provider = new UserRemoteProviderNew();

			#region Methods: Calls

			/*
			 * К сожалению, такой код не скомпилируется и выведет ошибку,
			 * [CS0106] The modifier 'override' is not valid for interface member declaration. Only 'new' is valid.
			 *
			 * На самом деле, необходимые нам модификации делаются по другому.
			 */

			GetUserByBaseInterfaceNew(provider);
			GetUserByHeadInterfaceNew(provider);

			/*
			 * Таким образом, полиморфизм и наследование работает и на методах в интерфейсах с реализацией по умолчанию.
			 */

			#endregion
		}

		#region Methods: Implementations

		static void GetUserByBaseInterfaceNew(UserRemoteProviderNew provider)
		{
			Console.Write("((IUserProvider) provider).GetUser(); - ");
			((IUserProvider) provider).GetUser();
		}

		static void GetUserByHeadInterfaceNew(UserRemoteProviderNew provider)
		{
			Console.Write("((IRemoteUserProviderNew) provider).GetUser(); - ");
			((IRemoteUserProviderNew) provider).GetUser();
		}

		#endregion

		#endregion

		#region Использование сразу пары интерфейсов

		static void ExampleFourth()
		{
			/*
			 * Использование сразу пары интерфейсов
			 *
			 * Думаю, что все прекрасно помнят, что в отличие от классов, наследоваться от нескольких интерфейсов можно.
			 * А что же будет, например, если мы реализуем сразу пару интерфейсов, каждый из которых переопределяет
			 * общий для этих интерфейсов метод с реализацией по умолчанию базового интерфейса.
			 * Т.е. фактически мы столкнемся с одной из проблем множественного наследования (Diamond inheritance).
			 */

			#region Methods: Calls

			/*
			 * Такой код не скомпилируется, т.к. компилятор не сможет найти более специфичный (т.е. ниже в иерархии наследования)
			 * интерфейс для использования, из-за чего не сможет понять, какой метод вызывать,
			 * если кто-то попробует вызвать метод GetUser().
			 *
			 * Таким образом, это не только крайне сомнительная возможность, имеющая мало юз-кейсов, но и
			 * довольно сильно усложняющая и так с каждым годом сложнеющий C#.
			 * По ощущениям, крайне маловероятно, что этой возможностью разработчики будут часто пользоваться.
			 */

			#endregion
		}

		#endregion
	}

	#region For ExampleFirst

	public interface IUserProvider
	{
		IUser GetUser()
		{
			Console.WriteLine("IUserProvider.GetUser()");
			return new User();
		}
	}

	public class UserProvider : IUserProvider
	{
	}

	#endregion

	#region For ExampleSecond

	public interface IRemoteUserProvider : IUserProvider
	{
		IUser GetUser()
		{
			Console.WriteLine("IRemoteUserProvider.GetUser()");
			return new User();
		}
	}

	public class UserRemoteProvider : IRemoteUserProvider
	{
	}

	#endregion

	#region For ExampleThird

	#region First example

	// public interface IUserProviderVirtual
	// {
	// 	virtual IUser GetUser()
	// 	{
	// 		Console.WriteLine("IUserProviderVirtual.GetUser()");
	// 		return new User();
	// 	}
	// }
	//
	// public interface IRemoteProviderOverriderFailer : IUserProviderVirtual
	// {
	// 	override IUser GetUser()
	// 	{
	// 		Console.WriteLine("IRemoteProviderOverrider.GetUser()");
	// 		return new User();
	// 	}
	// }

	#endregion

	#region Second example

	public interface IRemoteUserProviderNew : IUserProvider
	{
		IUser IUserProvider.GetUser()
		{
			Console.WriteLine("IRemoteUserProviderNew.GetUser()");
			return new User();
		}
	}

	#endregion

	public class UserRemoteProviderNew : IRemoteUserProviderNew
	{
	}

	#endregion

	#region For ExampleFourth

	public interface ILocalUserProviderNew : IUserProvider
	{
		IUser IUserProvider.GetUser()
		{
			Console.WriteLine("ILocalUserProviderNew.GetUser()");
			return new User();
		}
	}

	// public class LocalRemoteUserProvider : IRemoteUserProviderNew, ILocalUserProviderNew
	// {
	// }

	#endregion
}
