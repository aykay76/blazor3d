using System;

public class Matrix
{
    public float[] Values { get; set; }

    public Matrix()
    {
        Values = new float[16];

        for (int i = 0; i < 16; i++)
        {
            Values[i] = 0.0f;
        }

        Values[0] = 1.0f;
        Values[5] = 1.0f;
        Values[10] = 1.0f;
        Values[15] = 1.0f;
    }

    public static Matrix Perspective(float fovy, float aspect, float near, float far)
    {
        Matrix m = new Matrix();

        var f = 1.0 / Math.Tan(fovy / 2);

        m.Values[0] = (float)(f / aspect);
        m.Values[1] = 0.0f;
        m.Values[2] = 0.0f;
        m.Values[3] = 0.0f;
        m.Values[4] = 0.0f;
        m.Values[5] = (float)f;
        m.Values[6] = 0.0f;
        m.Values[7] = 0.0f;
        m.Values[8] = 0.0f;
        m.Values[9] = 0.0f;
        m.Values[11] = -1.0f;
        m.Values[12] = 0.0f;
        m.Values[13] = 0.0f;
        m.Values[15] = 0.0f;

        if (far == double.NaN) 
        {
            m.Values[10] = -1.0f;
            m.Values[14] = -2.0f * near;
        }
        else
        {
            var nf = 1.0f / (near - far);
            m.Values[10] = (far + near) * nf;
            m.Values[14] = 2.0f * far * near * nf;
        }
        return m;
    }

    public Matrix Invert(Matrix source)
    {
        var a00 = source.Values[0];
        var a01 = source.Values[1];
        var a02 = source.Values[2];
        var a03 = source.Values[3];
        var a10 = source.Values[4];
        var a11 = source.Values[5];
        var a12 = source.Values[6];
        var a13 = source.Values[7];
        var a20 = source.Values[8];
        var a21 = source.Values[9];
        var a22 = source.Values[10];
        var a23 = source.Values[11];
        var a30 = source.Values[12];
        var a31 = source.Values[13];
        var a32 = source.Values[14];
        var a33 = source.Values[15];

        var b00 = a00 * a11 - a01 * a10;
        var b01 = a00 * a12 - a02 * a10;
        var b02 = a00 * a13 - a03 * a10;
        var b03 = a01 * a12 - a02 * a11;
        var b04 = a01 * a13 - a03 * a11;
        var b05 = a02 * a13 - a03 * a12;
        var b06 = a20 * a31 - a21 * a30;
        var b07 = a20 * a32 - a22 * a30;
        var b08 = a20 * a33 - a23 * a30;
        var b09 = a21 * a32 - a22 * a31;
        var b10 = a21 * a33 - a23 * a31;
        var b11 = a22 * a33 - a23 * a32;

        // Calculate the determinant
        var det = b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06;

        if (det == 0.0f) {
            return this;
        }
        det = 1.0f / det;

        Values[0] = (a11 * b11 - a12 * b10 + a13 * b09) * det;
        Values[1] = (a02 * b10 - a01 * b11 - a03 * b09) * det;
        Values[2] = (a31 * b05 - a32 * b04 + a33 * b03) * det;
        Values[3] = (a22 * b04 - a21 * b05 - a23 * b03) * det;
        Values[4] = (a12 * b08 - a10 * b11 - a13 * b07) * det;
        Values[5] = (a00 * b11 - a02 * b08 + a03 * b07) * det;
        Values[6] = (a32 * b02 - a30 * b05 - a33 * b01) * det;
        Values[7] = (a20 * b05 - a22 * b02 + a23 * b01) * det;
        Values[8] = (a10 * b10 - a11 * b08 + a13 * b06) * det;
        Values[9] = (a01 * b08 - a00 * b10 - a03 * b06) * det;
        Values[10] = (a30 * b04 - a31 * b02 + a33 * b00) * det;
        Values[11] = (a21 * b02 - a20 * b04 - a23 * b00) * det;
        Values[12] = (a11 * b07 - a10 * b09 - a12 * b06) * det;
        Values[13] = (a00 * b09 - a01 * b07 + a02 * b06) * det;
        Values[14] = (a31 * b01 - a30 * b03 - a32 * b00) * det;
        Values[15] = (a20 * b03 - a21 * b01 + a22 * b00) * det;

        return this;
    }

    public Matrix Transpose()
    {
        var a01 = Values[1];
        var a02 = Values[2];
        var a03 = Values[3];
        var a12 = Values[6];
        var a13 = Values[7];
        var a23 = Values[11];

        Values[1] = Values[4];
        Values[2] = Values[8];
        Values[3] = Values[12];
        Values[4] = a01;
        Values[6] = Values[9];
        Values[7] = Values[13];
        Values[8] = a02;
        Values[9] = a12;
        Values[11] = Values[14];
        Values[12] = a03;
        Values[13] = a13;
        Values[14] = a23;

        return this;
    }

    public Matrix Translate(float x, float y, float z)
    {
        Values[12] = Values[0] * x + Values[4] * y + Values[8] * z + Values[12];
        Values[13] = Values[1] * x + Values[5] * y + Values[9] * z + Values[13];
        Values[14] = Values[2] * x + Values[6] * y + Values[10] * z + Values[14];
        Values[15] = Values[3] * x + Values[7] * y + Values[11] * z + Values[15];        
        return this;
    }

    public Matrix Scale(float x, float y, float z)
    {
        Values[0] = Values[0] * x;
        Values[1] = Values[1] * x;
        Values[2] = Values[2] * x;
        Values[3] = Values[3] * x;
        Values[4] = Values[4] * y;
        Values[5] = Values[5] * y;
        Values[6] = Values[6] * y;
        Values[7] = Values[7] * y;
        Values[8] = Values[8] * z;
        Values[9] = Values[9] * z;
        Values[10] = Values[10] * z;
        Values[11] = Values[11] * z;
        Values[12] = Values[12];
        Values[13] = Values[13];
        Values[14] = Values[14];
        Values[15] = Values[15];

        return this;
    }

    public Matrix Rotate(float angle, float x, float y, float z)
    {
        float len = (float)Math.Sqrt(x * x + y * y + z * z);
        float s, c, t;
        float a00, a01, a02, a03;
        float a10, a11, a12, a13;
        float a20, a21, a22, a23;
        float b00, b01, b02;
        float b10, b11, b12;
        float b20, b21, b22;

        if (len < float.MinValue) {
            return this;
        }

        len = 1.0f / len;
        x *= len;
        y *= len;
        z *= len;

        s = (float)Math.Sin(angle);
        c = (float)Math.Cos(angle);
        t = 1.0f - c;

        a00 = Values[0];
        a01 = Values[1];
        a02 = Values[2];
        a03 = Values[3];
        a10 = Values[4];
        a11 = Values[5];
        a12 = Values[6];
        a13 = Values[7];
        a20 = Values[8];
        a21 = Values[9];
        a22 = Values[10];
        a23 = Values[11];

        // Construct the elements of the rotation matrix
        b00 = x * x * t + c;
        b01 = y * x * t + z * s;
        b02 = z * x * t - y * s;
        b10 = x * y * t - z * s;
        b11 = y * y * t + c;
        b12 = z * y * t + x * s;
        b20 = x * z * t + y * s;
        b21 = y * z * t - x * s;
        b22 = z * z * t + c;

        // Perform rotation-specific matrix multiplication
        Values[0] = a00 * b00 + a10 * b01 + a20 * b02;
        Values[1] = a01 * b00 + a11 * b01 + a21 * b02;
        Values[2] = a02 * b00 + a12 * b01 + a22 * b02;
        Values[3] = a03 * b00 + a13 * b01 + a23 * b02;
        Values[4] = a00 * b10 + a10 * b11 + a20 * b12;
        Values[5] = a01 * b10 + a11 * b11 + a21 * b12;
        Values[6] = a02 * b10 + a12 * b11 + a22 * b12;
        Values[7] = a03 * b10 + a13 * b11 + a23 * b12;
        Values[8] = a00 * b20 + a10 * b21 + a20 * b22;
        Values[9] = a01 * b20 + a11 * b21 + a21 * b22;
        Values[10] = a02 * b20 + a12 * b21 + a22 * b22;
        Values[11] = a03 * b20 + a13 * b21 + a23 * b22;

        return this;
    }

    public void RotateX(float angle)
    {
        float s = (float)Math.Sin(angle);
        float c = (float)Math.Cos(angle);
        var a10 = Values[4];
        var a11 = Values[5];
        var a12 = Values[6];
        var a13 = Values[7];
        var a20 = Values[8];
        var a21 = Values[9];
        var a22 = Values[10];
        var a23 = Values[11];

        // Perform axis-specific matrix multiplication
        Values[4] = a10 * c + a20 * s;
        Values[5] = a11 * c + a21 * s;
        Values[6] = a12 * c + a22 * s;
        Values[7] = a13 * c + a23 * s;
        Values[8] = a20 * c - a10 * s;
        Values[9] = a21 * c - a11 * s;
        Values[10] = a22 * c - a12 * s;
        Values[11] = a23 * c - a13 * s;
    }

    public void RotateY(float angle)
    {
        float s = (float)Math.Sin(angle);
        float c = (float)Math.Cos(angle);
        var a00 = Values[0];
        var a01 = Values[1];
        var a02 = Values[2];
        var a03 = Values[3];
        var a20 = Values[8];
        var a21 = Values[9];
        var a22 = Values[10];
        var a23 = Values[11];

        // Perform axis-specific matrix multiplication
        Values[0] = a00 * c - a20 * s;
        Values[1] = a01 * c - a21 * s;
        Values[2] = a02 * c - a22 * s;
        Values[3] = a03 * c - a23 * s;
        Values[8] = a00 * s + a20 * c;
        Values[9] = a01 * s + a21 * c;
        Values[10] = a02 * s + a22 * c;
        Values[11] = a03 * s + a23 * c;
    }

    public void RotateZ(float angle)
    {
        float s = (float)Math.Sin(angle);
        float c = (float)Math.Cos(angle);
        var a00 = Values[0];
        var a01 = Values[1];
        var a02 = Values[2];
        var a03 = Values[3];
        var a10 = Values[4];
        var a11 = Values[5];
        var a12 = Values[6];
        var a13 = Values[7];

        // Perform axis-specific matrix multiplication
        Values[0] = a00 * c + a10 * s;
        Values[1] = a01 * c + a11 * s;
        Values[2] = a02 * c + a12 * s;
        Values[3] = a03 * c + a13 * s;
        Values[4] = a10 * c - a00 * s;
        Values[5] = a11 * c - a01 * s;
        Values[6] = a12 * c - a02 * s;
        Values[7] = a13 * c - a03 * s;        
    }
}