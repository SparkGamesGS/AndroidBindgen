// --------------------------------------------------------------
// Copyright 2025 CyberAgent, Inc.
// --------------------------------------------------------------

package jp.co.cyberagent.androidbindgen.testlib;

import java.util.Arrays;

public class TestStaticMembers {
    public static void testMethodVoid() {
    }

    public static byte testMethodByte(byte value) {
        return value;
    }

    public static char testMethodChar(char value) {
        return value;
    }

    public static double testMethodDouble(double value) {
        return value;
    }

    public static float testMethodFloat(float value) {
        return value;
    }

    public static int testMethodInt(int value) {
        return value;
    }

    public static long testMethodLong(long value) {
        return value;
    }

    public static short testMethodShort(short value) {
        return value;
    }

    public static boolean testMethodBoolean(boolean value) {
        return value;
    }

    public static Object testMethodObject(Object value) {
        return value;
    }

    public static String testMethodString(String value) {
        return value;
    }

    public static Object testMethodObjectReturnsNull() {
        return null;
    }

    public static byte[] testMethodByteArray(byte[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = (byte) (value[i] + 1);
        }
        return value;
    }

    public static char[] testMethodCharArray(char[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = (char) (value[i] + 1);
        }
        return value;
    }

    public static double[] testMethodDoubleArray(double[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = value[i] + 1;
        }
        return value;
    }

    public static float[] testMethodFloatArray(float[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = value[i] + 1;
        }
        return value;
    }

    public static int[] testMethodIntArray(int[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = value[i] + 1;
        }
        return value;
    }

    public static long[] testMethodLongArray(long[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = value[i] + 1;
        }
        return value;
    }

    public static short[] testMethodShortArray(short[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = (short) (value[i] + 1);
        }
        return value;
    }

    public static boolean[] testMethodBooleanArray(boolean[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = !value[i];
        }
        return value;
    }

    public static Object[] testMethodObjectArray(Object[] value) {
        Arrays.fill(value, null);
        return value;
    }

    public static String[] testMethodStringArray(String[] value) {
        for (int i = 0; i < value.length; i++) {
            value[i] = value[i] + "!";
        }
        return value;
    }

    public static Object[] testMethodObjectArrayReturnsNull() {
        return null;
    }

    public static int[][] testMethodJaggedIntArray(int[][] value) {
        for (int a = 0; a < value.length; a++) {
            for (int b = 0; b < value[a].length; b++) {
                value[a][b] = value[a][b] + 1;
            }
        }
        return value;
    }

    public static Object[][] testMethodJaggedObjectArray(Object[][] value) {
        for (int a = 0; a < value.length; a++) {
            Arrays.fill(value[a], null);
        }
        return value;
    }


    public static byte testFieldByte;
    public static char testFieldChar;
    public static double testFieldDouble;
    public static float testFieldFloat;
    public static int testFieldInt;
    public static long testFieldLong;
    public static short testFieldShort;
    public static boolean testFieldBoolean;

    public static String testFieldString;
    public static Object testFieldObject;

    public static byte[] testFieldByteArray;

    public static char[] testFieldCharArray;

    public static double[] testFieldDoubleArray;

    public static float[] testFieldFloatArray;

    public static int[] testFieldIntArray;

    public static long[] testFieldLongArray;

    public static short[] testFieldShortArray;

    public static boolean[] testFieldBooleanArray;

    public static String[] testFieldStringArray;

    public static Object[] testFieldObjectArray;

    public static int[][] testFieldJaggedIntArray;

    public static Object[][] testFieldJaggedObjectArray;
}
