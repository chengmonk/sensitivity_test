using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HslCommunication.Algorithms.Fourier
{
    /// <summary>
    /// 一个基于傅立叶变换的一个滤波算法
    /// </summary>
    /// <remarks>
    /// 非常感谢来自北京的monk网友，提供了完整的解决方法。
    /// </remarks>
    public class FFTFilter
    {
        /// <summary>
        /// 对指定的数据进行填充，方便的进行傅立叶计算
        /// </summary>
        /// <typeparam name="T">数据的数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="putLength">输出的长度</param>
        /// <returns>填充结果</returns>
        public static List<T> FillDataArray<T>( List<T> source, out int putLength)
        {
            int length = (int)(Math.Pow( 2d, Convert.ToString( source.Count, 2 ).Length ) - source.Count);
            length = length / 2 + 1;
            putLength = length;
            T begin = source[1];
            T end = source[source.Count - 1];
            for (int i = 0; i < length; i++)
            {
                source.Insert( 0, begin );
            }
            for (int i = 0; i < length; i++)
            {
                source.Add( end );
            }
            return source;
        }

        /// <summary>
        /// 对指定的原始数据进行滤波，并返回成功的数据值
        /// </summary>
        /// <param name="source">数据源，数组的长度需要为2的n次方。</param>
        /// <param name="filter">滤波值：最大值为1，不能低于0，越接近0，滤波强度越强，也可能会导致失去真实信号，为1时没有滤波效果。</param>
        /// <returns>滤波后的数据值</returns>
        public static double[] FilterFFT( double[] source, double filter )
        {
            int reslen = source.Length;
            List<double> Y = new List<double>( source );
            Y = FillDataArray( Y, out int putlen );
            Console.WriteLine( "b:" + Y.ToArray( ).Length );
            float[] y = new float[Y.ToArray( ).Length];
            y = Filter( Y.ToArray( ), filter );
            for (int i = 0; i < reslen; i++)
            {
                source[i] = Math.Round( y[i + putlen], 2 );
            }
            return source;
        }

        /// <summary>
        /// 对指定的原始数据进行滤波，并返回成功的数据值
        /// </summary>
        /// <param name="source">数据源，数组的长度需要为2的n次方。</param>
        /// <param name="filter">滤波值：最大值为1，不能低于0，越接近0，滤波强度越强，也可能会导致失去真实信号，为1时没有滤波效果。</param>
        /// <returns>滤波后的数据值</returns>
        public static float[] FilterFFT( float[] source, double filter )
        {
            int reslen = source.Length;
            List<float> Y = new List<float>( source );
            Y = FillDataArray( Y, out int putlen );
            float[] y = new float[Y.Count];
            y = Filter( Y.ToArray( ), filter );
            for (int i = 0; i < reslen; i++)
            {
                source[i] = (float)Math.Round( y[i + putlen], 2 );
            }
            return source;
        }

        #region

        /// <summary>
        /// 对指定的原始数据进行滤波，并返回成功的数据值
        /// </summary>
        /// <param name="source">数据源，数组的长度需要为2的n次方。</param>
        /// <param name="filter">滤波值：最大值为1，不能低于0，越接近0，滤波强度越强，也可能会导致失去真实信号，为1时没有滤波效果。</param>
        /// <returns>滤波后的数据值</returns>
        private static float[] Filter( float[] source, double filter )
        {
            return Filter( source.Select( m => (double)m ).ToArray( ), filter );
        }

        /// <summary>
        /// 对指定的原始数据进行滤波，并返回成功的数据值
        /// </summary>
        /// <param name="source">数据源，数组的长度需要为2的n次方。</param>
        /// <param name="filter">滤波值：最大值为1，不能低于0，越接近0，滤波强度越强，也可能会导致失去真实信号，为1时没有滤波效果。</param>
        /// <returns>滤波后的数据值</returns>
        private static float[] Filter( double[] source, double filter )
        {
            int length = 0;
            // a是实部、b是虚部
            float[] a = new float[source.Length];
            float[] b = new float[source.Length];
            List<float> ans = new List<float>( );
            for (int j = 0; j < source.Length; j++)
            {
                a[j] = (float)source[j];
                b[j] = 0.0f;
            }
            length = FFTHelper.FFT( a, b );

            // length是傅里叶变换处理过后的数组大小 在频域上对数据进行过滤 
            int p = (int)(length * filter);
            for (int i = 0; i < length; i++)
            {
                if (p < i && i < length - p)
                {
                    a[i] = 0f;
                    b[i] = 0f;
                }
            }

            length = FFTHelper.IFFT( a, b );
            for (int i = 0; i < length; i++)
            {
                ans.Add( (float)Math.Sqrt( a[i] * a[i] + b[i] * b[i] ) );
            }

            return ans.ToArray( );
        }

        #endregion
    }
}
