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

    public void Translate(float x, float y, float z)
    {
        Values[12] = Values[0] * x + Values[4] * y + Values[8] * z + Values[12];
        Values[13] = Values[1] * x + Values[5] * y + Values[9] * z + Values[13];
        Values[14] = Values[2] * x + Values[6] * y + Values[10] * z + Values[14];
        Values[15] = Values[3] * x + Values[7] * y + Values[11] * z + Values[15];        
    }
}