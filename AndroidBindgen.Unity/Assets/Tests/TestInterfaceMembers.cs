// --------------------------------------------------------------
// Copyright 2024 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using NUnit.Framework;
using Object = java.lang.Object;

namespace Tests
{
    public class TestInterfaceMembers : jp.co.cyberagent.androidbindgen.testlib.TestInterfaceMembers.ProxyBase
    {
        public override void testMethodVoid()
        {
        }

        public override sbyte testMethodByte(sbyte value) => value;

        public override char testMethodChar(char value) => value;

        public override double testMethodDouble(double value) => value;

        public override float testMethodFloat(float value) => value;

        public override int testMethodInt(int value) => value;

        public override long testMethodLong(long value) => value;

        public override short testMethodShort(short value) => value;

        public override bool testMethodBoolean(bool value) => value;

        public override Object testMethodObject(Object value) => value;

        public override string testMethodString(string value) => value;

        public override Object testMethodObjectReturnsNull() => null;

        public override sbyte[] testMethodByteArray(sbyte[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = (sbyte)(value[i] + 1);
            }

            return value;
        }

        public override char[] testMethodCharArray(char[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = (char)(value[i] + 1);
            }

            return value;
        }

        public override double[] testMethodDoubleArray(double[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = (double)(value[i] + 1);
            }

            return value;
        }

        public override float[] testMethodFloatArray(float[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = (float)(value[i] + 1);
            }

            return value;
        }

        public override int[] testMethodIntArray(int[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = (int)(value[i] + 1);
            }

            return value;
        }

        public override long[] testMethodLongArray(long[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = (long)(value[i] + 1);
            }

            return value;
        }

        public override short[] testMethodShortArray(short[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = (short)(value[i] + 1);
            }

            return value;
        }

        public override bool[] testMethodBooleanArray(bool[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = !value[i];
            }

            return value;
        }

        public override Object[] testMethodObjectArray(Object[] value)
        {
            Array.Clear(value, 0, value.Length);
            return value;
        }

        public override string[] testMethodStringArray(string[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = value[i] + "!";
            }

            return value;
        }

        public override Object[] testMethodObjectArrayReturnsNull()
        {
            return null;
        }

        public override int[][] testMethodJaggedIntArray(int[][] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                for (int j = 0; j < value[i].Length; j++)
                {
                    value[i][j] = value[i][j] + 1;
                }
            }

            return value;
        }

        public override Object[][] testMethodJaggedObjectArray(Object[][] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                for (int j = 0; j < value[i].Length; j++)
                {
                    value[i][j] = null;
                }
            }

            return value;
        }
    }
}
