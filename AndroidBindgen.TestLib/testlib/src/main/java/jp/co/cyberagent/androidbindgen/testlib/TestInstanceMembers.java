// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

package jp.co.cyberagent.androidbindgen.testlib;

import java.util.Arrays;

public class TestInstanceMembers implements TestInterfaceMembers {
    public void testMethodVoid() {
    }

    public byte testMethodByte(byte value) {
        return value;
    }

    public char testMethodChar(char value) {
        return value;
    }

    public double testMethodDouble(double value) {
        return value;
    }

    public float testMethodFloat(float value) {
        return value;
    }

    public int testMethodInt(int value) {
        return value;
    }

    public long testMethodLong(long value) {
        return value;
    }

    public short testMethodShort(short value) {
        return value;
    }

    public boolean testMethodBoolean(boolean value) {
        return value;
    }

    public Object testMethodObject(Object value) {
        return value;
    }

    public String testMethodString(String value) {
        return value;
    }

    public Object testMethodObjectReturnsNull() {
        return null;
    }

    public byte[] testMethodByteArray(byte[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = (byte) (value[i] + 1);
        }
        return value;
    }

    public char[] testMethodCharArray(char[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = (char) (value[i] + 1);
        }
        return value;
    }

    public double[] testMethodDoubleArray(double[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = value[i] + 1;
        }
        return value;
    }

    public float[] testMethodFloatArray(float[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = value[i] + 1;
        }
        return value;
    }

    public int[] testMethodIntArray(int[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = value[i] + 1;
        }
        return value;
    }

    public long[] testMethodLongArray(long[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = value[i] + 1;
        }
        return value;
    }

    public short[] testMethodShortArray(short[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = (short) (value[i] + 1);
        }
        return value;
    }

    public boolean[] testMethodBooleanArray(boolean[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = !value[i];
        }
        return value;
    }

    public Object[] testMethodObjectArray(Object[] value) {
        Arrays.fill(value, null);
        return value;
    }

    public String[] testMethodStringArray(String[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = value[i] + "!";
        }
        return value;
    }

    public Object[] testMethodObjectArrayReturnsNull() {
        return null;
    }

    public int[][] testMethodJaggedIntArray(int[][] value) {
        for (int a = 0; a < value.length; a++) {
            for (int b = 0; b < value[a].length; b++) {
                value[a][b] = value[a][b] + 1;
            }
        }
        return value;
    }

    public Object[][] testMethodJaggedObjectArray(Object[][] value) {
        for (int a = 0; a < value.length; a++) {
            Arrays.fill(value[a], null);
        }
        return value;
    }


    public byte testFieldByte;
    public char testFieldChar;
    public double testFieldDouble;
    public float testFieldFloat;
    public int testFieldInt;
    public long testFieldLong;
    public short testFieldShort;
    public boolean testFieldBoolean;

    public String testFieldString;
    public Object testFieldObject;

    public byte[] testFieldByteArray;

    public char[] testFieldCharArray;

    public double[] testFieldDoubleArray;

    public float[] testFieldFloatArray;

    public int[] testFieldIntArray;

    public long[] testFieldLongArray;

    public short[] testFieldShortArray;

    public boolean[] testFieldBooleanArray;

    public String[] testFieldStringArray;

    public Object[] testFieldObjectArray;

    public int[][] testFieldJaggedIntArray;

    public Object[][] testFieldJaggedObjectArray;
}
