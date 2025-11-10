using System.Linq;
using java.lang;
using jp.co.cyberagent.androidbindgen.testlib;
using NUnit.Framework;
using UnityEngine;
using Object = java.lang.Object;
using _target = jp.co.cyberagent.androidbindgen.testlib.TestStaticMembers;

public class TestStaticMembers
{
    [Test]
    public void TestMethodVoid()
    {
        _target.testMethodVoid();
    }

    [Test]
    public void TestMethodByte([Random(sbyte.MinValue, sbyte.MaxValue, 10)] sbyte value)
    {
        Assert.That(_target.testMethodByte(value), Is.EqualTo(value));
    }

    [Test]
    public void TestMethodChar()
    {
        Assert.That(_target.testMethodChar('あ'), Is.EqualTo('あ'));
    }

    [Test]
    public void TestMethodDouble([Random(double.MinValue, double.MaxValue, 10)] double value)
    {
        Assert.That(_target.testMethodDouble(value), Is.EqualTo(value));
    }

    [Test]
    public void TestMethodFloat([Random(float.MinValue, float.MaxValue, 10)] float value)
    {
        Assert.That(_target.testMethodFloat(value), Is.EqualTo(value));
    }

    [Test]
    public void TestMethodInt([Random(int.MinValue, int.MaxValue, 10)] int value)
    {
        Assert.That(_target.testMethodInt(value), Is.EqualTo(value));
    }

    [Test]
    public void TestMethodLong([Random(long.MinValue, long.MaxValue, 10)] long value)
    {
        Assert.That(_target.testMethodLong(value), Is.EqualTo(value));
    }

    [Test]
    public void TestMethodShort([Random(short.MinValue, short.MaxValue, 10)] short value)
    {
        Assert.That(_target.testMethodShort(value), Is.EqualTo(value));
    }

    [Test]
    public void TestMethodBoolean([Values(false, true)] bool value)
    {
        Assert.That(_target.testMethodBoolean(value), Is.EqualTo(value));
    }

    [Test]
    public void TestMethodObject()
    {
        var obj = Object.New();
        var returned = _target.testMethodObject(obj);
        Assert.That(obj.equals(returned), Is.True);
    }

    [Test]
    public void TestMethodString()
    {
        Assert.That(_target.testMethodString("Lorem ipsum"), Is.EqualTo("Lorem ipsum"));
    }

    [Test]
    public void TestMethodObjectReturnsNull()
    {
        var returned = _target.testMethodObjectReturnsNull();
        Assert.That(returned, Is.Null);
    }

    [Test]
    public void TestMethodByteArray()
    {
        var source = Enumerable.Range(0, 100).Select(v => unchecked((sbyte)v)).ToArray();
        var expected = source.Select(v => (sbyte)(v + 1)).ToArray();

        var returned = _target.testMethodByteArray(source);
        Assert.That(returned, Is.EquivalentTo(expected));
        Assert.That(source, Is.EquivalentTo(expected));
    }


    [Test]
    public void TestMethodCharArray()
    {
        var source = Enumerable.Range(0, 100).Select(v => (char)v).ToArray();
        var expected = source.Select(v => (char)(v + 1)).ToArray();

        var returned = _target.testMethodCharArray(source);
        Assert.That(returned, Is.EquivalentTo(expected));
        Assert.That(source, Is.EquivalentTo(expected));
    }

    [Test]
    public void TestMethodDoubleArray()
    {
        var source = Enumerable.Range(0, 100).Select(v => (double)v).ToArray();
        var expected = source.Select(v => v + 1).ToArray();

        var returned = _target.testMethodDoubleArray(source);
        Assert.That(returned, Is.EquivalentTo(expected));
        Assert.That(source, Is.EquivalentTo(expected));
    }

    [Test]
    public void TestMethodFloatArray()
    {
        var source = Enumerable.Range(0, 100).Select(v => (float)v).ToArray();
        var expected = source.Select(v => v + 1).ToArray();

        var returned = _target.testMethodFloatArray(source);
        Assert.That(returned, Is.EquivalentTo(expected));
        Assert.That(source, Is.EquivalentTo(expected));
    }

    [Test]
    public void TestMethodIntArray()
    {
        var source = Enumerable.Range(0, 100).ToArray();
        var expected = source.Select(v => v + 1).ToArray();

        var returned = _target.testMethodIntArray(source);
        Assert.That(returned, Is.EquivalentTo(expected));
        Assert.That(source, Is.EquivalentTo(expected));
    }

    [Test]
    public void TestMethodLongArray()
    {
        var source = Enumerable.Range(0, 100).Select(v => (long)v).ToArray();
        var expected = source.Select(v => v + 1).ToArray();

        var returned = _target.testMethodLongArray(source);
        Assert.That(returned, Is.EquivalentTo(expected));
        Assert.That(source, Is.EquivalentTo(expected));
    }

    [Test]
    public void TestMethodShortArray()
    {
        var source = Enumerable.Range(0, 100).Select(v => (short)v).ToArray();
        var expected = source.Select(v => (short)(v + 1)).ToArray();

        var returned = _target.testMethodShortArray(source);
        Assert.That(returned, Is.EquivalentTo(expected));
        Assert.That(source, Is.EquivalentTo(expected));
    }

    [Test]
    public void TestMethodBooleanArray()
    {
        var source = Enumerable.Range(0, 100).Select(v => v % 2 == 0).ToArray();
        var expected = source.Select(v => !v).ToArray();

        var returned = _target.testMethodBooleanArray(source);
        Assert.That(returned, Is.EquivalentTo(expected));
        Assert.That(source, Is.EquivalentTo(expected));
    }

    [Test]
    public void TestMethodStringArray()
    {
        var source = Enumerable.Range(0, 100).Select(v => "hoge").ToArray();
        var expected = source.Select(v => "hoge!").ToArray();

        var returned = _target.testMethodStringArray(source);
        Assert.That(returned, Is.EquivalentTo(expected));
        Assert.That(source, Is.EquivalentTo(expected));
    }

    [Test]
    public void TestMethodObjectArray()
    {
        var source = Enumerable.Range(0, 100).Select(v => Object.New()).ToArray();
        var expected = source.Select<Object, Object>(v => null).ToArray();

        var returned = _target.testMethodObjectArray(source);
        Assert.That(returned, Is.EquivalentTo(expected));
        Assert.That(source, Is.EquivalentTo(expected));
    }

    [Test]
    public void TestMethodJaggedIntArray()
    {
        var source = Enumerable.Range(0, 10).Select(x => Enumerable.Range(0, 10).Select(y => x + y).ToArray())
            .ToArray();
        var expected = source.Select(l => l.Select(e => e + 1).ToArray()).ToArray();
        var returned = _target.testMethodJaggedIntArray(source);

        Assert.That(returned, Is.Not.Null);

        Assert.That(returned.Length, Is.EqualTo(expected.Length));
        Assert.That(source.Length, Is.EqualTo(expected.Length));

        for (var i = 0; i < source.Length; i++)
        {
            Assert.That(returned[i], Is.EquivalentTo(expected[i]));
            Assert.That(source[i], Is.EquivalentTo(expected[i]));
        }
    }

    [Test]
    public void TestMethodJaggedObjectArray()
    {
        var source = Enumerable.Range(0, 10).Select(x => Enumerable.Range(0, 10).Select(y => Object.New()).ToArray())
            .ToArray();
        var expectedLength = source.Length;
        var returned = _target.testMethodJaggedObjectArray(source);

        Assert.That(returned, Is.Not.Null);

        Assert.That(returned.Length, Is.EqualTo(expectedLength));
        Assert.That(source.Length, Is.EqualTo(expectedLength));

        for (var i = 0; i < source.Length; i++)
        {
            Assert.That(returned[i], Is.All.Null);
            Assert.That(source[i], Is.All.Null);
        }
    }

    [Test]
    public void TestMethodObjectArrayReturnsNull()
    {
        var returned = _target.testMethodObjectArrayReturnsNull();
        Assert.That(returned, Is.Null);
    }

    [Test]
    public void TestFieldByte([Random(sbyte.MinValue, sbyte.MaxValue, 10)] sbyte value)
    {
        _target.set_testFieldByte(value);
        Assert.That(_target.get_testFieldByte(), Is.EqualTo(value));
    }

    [Test]
    public void TestFieldChar()
    {
        _target.set_testFieldChar('あ');
        Assert.That(_target.get_testFieldChar(), Is.EqualTo('あ'));
    }

    [Test]
    public void TestFieldDouble([Random(double.MinValue, double.MaxValue, 10)] double value)
    {
        _target.set_testFieldDouble(value);
        Assert.That(_target.get_testFieldDouble(), Is.EqualTo(value));
    }

    [Test]
    public void TestFieldFloat([Random(float.MinValue, float.MaxValue, 10)] float value)
    {
        _target.set_testFieldFloat(value);
        Assert.That(_target.get_testFieldFloat(), Is.EqualTo(value));
    }

    [Test]
    public void TestFieldInt([Random(int.MinValue, int.MaxValue, 10)] int value)
    {
        _target.set_testFieldInt(value);
        Assert.That(_target.get_testFieldInt(), Is.EqualTo(value));
    }

    [Test]
    public void TestFieldLong([Random(long.MinValue, long.MaxValue, 10)] long value)
    {
        _target.set_testFieldLong(value);
        Assert.That(_target.get_testFieldLong(), Is.EqualTo(value));
    }

    [Test]
    public void TestFieldShort([Random(short.MinValue, short.MaxValue, 10)] short value)
    {
        _target.set_testFieldShort(value);
        Assert.That(_target.get_testFieldShort(), Is.EqualTo(value));
    }

    [Test]
    public void TestFieldBoolean([Values(false, true)] bool value)
    {
        _target.set_testFieldBoolean(value);
        Assert.That(_target.get_testFieldBoolean(), Is.EqualTo(value));
    }

    [Test]
    public void TestFieldString()
    {
        _target.set_testFieldString(null);
        Assert.That(_target.get_testFieldString(), Is.Null);

        _target.set_testFieldString("Lorem ipsum");
        Assert.That(_target.get_testFieldString(), Is.EqualTo("Lorem ipsum"));

        _target.set_testFieldString(null);
        Assert.That(_target.get_testFieldString(), Is.Null);
    }

    [Test]
    public void TestFieldObject()
    {
        _target.set_testFieldObject(null);
        Assert.That(_target.get_testFieldObject(), Is.Null);

        var obj = Object.New();
        _target.set_testFieldObject(obj);
        Assert.That(obj.equals(_target.get_testFieldObject()), Is.True);

        _target.set_testFieldObject(null);
        Assert.That(_target.get_testFieldObject(), Is.Null);
    }

    [Test]
    public void TestFieldByteArray()
    {
        _target.set_testFieldByteArray(null);
        Assert.That(_target.get_testFieldByteArray(), Is.Null);

        var source = Enumerable.Range(0, 100).Select(v => unchecked((sbyte)v)).ToArray();

        _target.set_testFieldByteArray(source);
        Assert.That(_target.get_testFieldByteArray(), Is.EquivalentTo(source));

        _target.set_testFieldByteArray(null);
        Assert.That(_target.get_testFieldByteArray(), Is.Null);
    }

    [Test]
    public void TestFieldCharArray()
    {
        _target.set_testFieldCharArray(null);
        Assert.That(_target.get_testFieldCharArray(), Is.Null);

        var source = Enumerable.Range(0, 100).Select(v => (char)v).ToArray();

        _target.set_testFieldCharArray(source);
        Assert.That(_target.get_testFieldCharArray(), Is.EquivalentTo(source));

        _target.set_testFieldCharArray(null);
        Assert.That(_target.get_testFieldCharArray(), Is.Null);
    }

    [Test]
    public void TestFieldDoubleArray()
    {
        _target.set_testFieldDoubleArray(null);
        Assert.That(_target.get_testFieldDoubleArray(), Is.Null);

        var source = Enumerable.Range(0, 100).Select(v => (double)v).ToArray();

        _target.set_testFieldDoubleArray(source);
        Assert.That(_target.get_testFieldDoubleArray(), Is.EquivalentTo(source));

        _target.set_testFieldDoubleArray(null);
        Assert.That(_target.get_testFieldDoubleArray(), Is.Null);
    }

    [Test]
    public void TestFieldFloatArray()
    {
        _target.set_testFieldFloatArray(null);
        Assert.That(_target.get_testFieldFloatArray(), Is.Null);

        var source = Enumerable.Range(0, 100).Select(v => (float)v).ToArray();

        _target.set_testFieldFloatArray(source);
        Assert.That(_target.get_testFieldFloatArray(), Is.EquivalentTo(source));

        _target.set_testFieldFloatArray(null);
        Assert.That(_target.get_testFieldFloatArray(), Is.Null);
    }

    [Test]
    public void TestFieldIntArray()
    {
        _target.set_testFieldIntArray(null);
        Assert.That(_target.get_testFieldIntArray(), Is.Null);

        var source = Enumerable.Range(0, 100).ToArray();

        _target.set_testFieldIntArray(source);
        Assert.That(_target.get_testFieldIntArray(), Is.EquivalentTo(source));

        _target.set_testFieldIntArray(null);
        Assert.That(_target.get_testFieldIntArray(), Is.Null);
    }

    [Test]
    public void TestFieldLongArray()
    {
        _target.set_testFieldLongArray(null);
        Assert.That(_target.get_testFieldLongArray(), Is.Null);

        var source = Enumerable.Range(0, 100).Select(v => (long)v).ToArray();

        _target.set_testFieldLongArray(source);
        Assert.That(_target.get_testFieldLongArray(), Is.EquivalentTo(source));

        _target.set_testFieldLongArray(null);
        Assert.That(_target.get_testFieldLongArray(), Is.Null);
    }

    [Test]
    public void TestFieldShortArray()
    {
        _target.set_testFieldShortArray(null);
        Assert.That(_target.get_testFieldShortArray(), Is.Null);

        var source = Enumerable.Range(0, 100).Select(v => (short)v).ToArray();

        _target.set_testFieldShortArray(source);
        Assert.That(_target.get_testFieldShortArray(), Is.EquivalentTo(source));

        _target.set_testFieldShortArray(null);
        Assert.That(_target.get_testFieldShortArray(), Is.Null);
    }

    [Test]
    public void TestFieldBooleanArray()
    {
        Debug.Log("Reset");
        _target.set_testFieldBooleanArray(null);
        Assert.That(_target.get_testFieldBooleanArray(), Is.Null);

        Debug.Log("Set");
        var source = Enumerable.Range(0, 100).Select(v => v % 2 == 0).ToArray();

        _target.set_testFieldBooleanArray(source);
        Assert.That(_target.get_testFieldBooleanArray(), Is.EquivalentTo(source));

        Debug.Log("Reset");
        _target.set_testFieldBooleanArray(null);
        Assert.That(_target.get_testFieldBooleanArray(), Is.Null);
    }

    [Test]
    public void TestFieldStringArray()
    {
        _target.set_testFieldStringArray(null);
        Assert.That(_target.get_testFieldStringArray(), Is.Null);

        var source = Enumerable.Range(0, 100).Select(v => "hoge").ToArray();

        _target.set_testFieldStringArray(source);
        Assert.That(_target.get_testFieldStringArray(), Is.EquivalentTo(source));

        _target.set_testFieldStringArray(null);
        Assert.That(_target.get_testFieldStringArray(), Is.Null);
    }

    [Test]
    public void TestFieldObjectArray()
    {
        _target.set_testFieldObjectArray(null);
        Assert.That(_target.get_testFieldObjectArray(), Is.Null);

        var source = Enumerable.Range(0, 100).Select(v => Object.New()).ToArray();

        _target.set_testFieldObjectArray(source);
        var returned = _target.get_testFieldObjectArray();
        Assert.That(returned!.Length, Is.EqualTo(source.Length));
        for (var i = 0; i < source.Length; i++)
            Assert.That(source[i].equals(returned[i]), Is.True);

        _target.set_testFieldObjectArray(null);
        Assert.That(_target.get_testFieldObjectArray(), Is.Null);
    }

    [Test]
    public void TestFieldJaggedIntArray()
    {   
        _target.set_testFieldJaggedIntArray(null);
        Assert.That(_target.get_testFieldJaggedIntArray(), Is.Null);

        var source = Enumerable.Range(0, 10).Select(x => Enumerable.Range(0, 10).Select(y => x + y).ToArray())
            .ToArray();

        _target.set_testFieldJaggedIntArray(source);
        var returned = _target.get_testFieldJaggedIntArray();
        
        Assert.That(returned, Is.Not.Null);

        Assert.That(returned.Length, Is.EqualTo(source.Length));

        for (var i = 0; i < source.Length; i++)
        {
            Assert.That(returned[i], Is.EquivalentTo(source[i]));
        }
        
        _target.set_testFieldJaggedIntArray(null);
        Assert.That(_target.get_testFieldJaggedIntArray(), Is.Null);
    }
    
    [Test]
    public void TestFieldJaggedObjectArray()
    {   
        _target.set_testFieldJaggedObjectArray(null);
        Assert.That(_target.get_testFieldJaggedObjectArray(), Is.Null);

        var source = Enumerable.Range(0, 10).Select(x => Enumerable.Range(0, 10).Select(y => Object.New()).ToArray())
            .ToArray();

        _target.set_testFieldJaggedObjectArray(source);
        var returned = _target.get_testFieldJaggedObjectArray();
        
        Assert.That(returned, Is.Not.Null);

        Assert.That(returned.Length, Is.EqualTo(source.Length));

        for (var i = 0; i < source.Length; i++)
        {
            Assert.That(returned[i].Length, Is.EqualTo(source[i].Length));
            for (var j = 0; j < returned.Length; j++)
            {
                Assert.That(returned[i][j].equals(source[i][j]), Is.True);
            }
        }
        
        _target.set_testFieldJaggedObjectArray(null);
        Assert.That(_target.get_testFieldJaggedObjectArray(), Is.Null);
    }
}
