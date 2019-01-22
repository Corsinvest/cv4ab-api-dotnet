# Corsinvest.AllenBradley.PLC.Api

[![License](https://img.shields.io/github/license/Corsinvest/cv4ab-api-dotnet.svg)](https://www.gnu.org/licenses/gpl-3.0.en.html)

Comunication for Allen-Bradley Rockwell PLC in .NET Core

[LibTagPLC library C++ Api](https://github.com/kyle-github/libplctag)

[Based on libplctag-csharp](https://github.com/mesta1/libplctag-csharp)

[Nuget](https://www.nuget.org/packages/Corsinvest.AllenBradley.PLC.Api)

[Special tanks for testing Mavert](https://www.mavert.it)

Special thanks to Mario Averoldi for technical support <mario.averoldi@mavert.it>.

[Corsinvest Srl](https://www.corsinvest.it)

[![AppVeyor branch](https://img.shields.io/appveyor/ci/franklupo/cv4ab-api-dotnet/master.svg)](https://ci.appveyor.com/project/franklupo/cv4ab-api-dotnet)

```text
   ______                _                      __
  / ____/___  __________(_)___ _   _____  _____/ /_
 / /   / __ \/ ___/ ___/ / __ \ | / / _ \/ ___/ __/
/ /___/ /_/ / /  (__  ) / / / / |/ /  __(__  ) /_
\____/\____/_/  /____/_/_/ /_/|___/\___/____/\__/

Client Api Allen-Bradley PLC          (Made in Italy)
```

## WARNING - DISCLAIMER

Note: *PLCs control many kinds of equipment and loss of property, production
or even life can happen if mistakes in programming or access are
made.  Always use caution when accessing or programming PLCs!*

We make no claims or warrants about the suitability of this code for
any purpose.

Be careful!

## General

The client is wapper of LibTagPLC library.

## Main features

- Open source
- Controller implementation
- Native Tag type INT8, UINT8, INT16, UINT16, INT32, UINT32, INT64, UINT64, FLOAT32, FLOAT64, STRING
- Custom class definition structure
- Manupulation local value variable
- Read and Write with advanced result
  - Time execution
  - Status code
  - Timestamp
  - Tag
- Value property decode value natively
- Lock/Unlock for thread operation
- Decode Error **StatusCodeOperation.DecodeError()**
- Group interval read/write
- Event result Tag and TagGroup with result changed value
- Enable "Fail Operation Raise Exception"
- Value Manager directry modify
- Bit manipulation
- Debug level
- Auto Read/Write when using value (default: false)

## Usage

```CSharp
[Serializable]
public class Test12
{
    public int AA1 { get; set; }
    public int AA2 { get; set; }
    public int AA3 { get; set; }
    public int AA4 { get; set; }
    public int AA5 { get; set; }
    public int AA6 { get; set; }
    public int AA7 { get; set; }
    public int AA8 { get; set; }
}

public static void Main(string[] args)
{
    //initialize controller
    using (var controller = new Controller("10.155.128.192", "1, 0", CPUType.LGX))
    {
        //ping controller
        Console.Out.WriteLine("Ping " + controller.Ping(true));

        //create group tag
        var grp = controller.CreateGroup();

        //add tag
        var tag = grp.CreateTagType<string[]>("Track", TagSize.STRING, 300);
        tag.Changed += TagChanged;
        var value = tag.Read();

        //add tag from Class
        var tag1 = grp.CreateTagType<Test12>("Test");
        tag.Changed += TagChanged;

        var tag2 = grp.CreateTagFloat32("Fl32");

        grp.Changed += GroupChanged;
        grp.Read();

    }
}

private static void PrintChange(string @event, ResultOperation result)
{
  Console.Out.WriteLine($"{@event} {result.Timestamp} Changed: {result.Tag.Name}");
}

static void TagChanged(ResultOperation result)
{
  PrintChange("TagChanged", result);
}

static void GroupChanged(IEnumerable<ResultOperation> results)
{
  foreach (var result in results) PrintChange("GroupTagChanged", result);
}
```

## Create Tag

Are possible to create any tag:

- CreateTagInt64
- CreateTagUInt64
- CreateTagInt32
- CreateTagUInt32
- CreateTagInt16
- CreateTagUInt16
- CreateTagInt8
- CreateTagUInt8
- CreateTagString
- CreateTagFloat32
- CreateTagFloat64
- CreateTagType specify type and name only, and automatcly calculated size from property or array
- CreateTagType specify name,size,length for array
- CreateTagArray

Size are specified in [TagSize](https://github.com/Corsinvest/cv4ab-api-dotnet/blob/master/src/Corsinvest.AllenBradley.PLC.Api/TagSize.cs).

For array specify size in definition.

Example:

```CSharp
public class TestArray
{
  public int InTest { get; set; }
  public int[] InTestArray { get; set; } = new int[5];
  public string[] StringTestArray { get; set; } = new string[300];
}
```

Custom type are class. The properties are read sequentially.