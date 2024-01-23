using System.Data;
using OSGeo.GDAL;

namespace DEMDemo
{
    internal class Program
    {
        static void Main()
        {
            // 初始化 GDAL
            Gdal.AllRegister();

            // ASTER GDEM文件路径
            string asterGdemFilePath = "C:\\Users\\Administrator\\Desktop\\3dloupan\\ASTGTM2_N29E106_dem.tif";

            // 打开ASTER GDEM文件
            Dataset ds = Gdal.Open(asterGdemFilePath, Access.GA_ReadOnly);
            if (ds == null)
            {
                Console.WriteLine("Failed to open ASTER GDEM file.");
                return;
            }

            // 获取经纬度对应的高程
            //double longitude = 106.56536974434377; // 你的经度
            //double latitude = 29.644273358829857;  // 你的纬度
            double longitude = 106.567596; // 你的经度
            double latitude = 29.644844;  // 你的纬度
            //double longitude = 651521.5331243385; // 你的经度
            //double latitude = 3280392.95919055;  // 你的纬度
            double elevation = GetElevationFromCoordinates(longitude, latitude, ds);

            // 输出高程值
            Console.WriteLine($"Elevation at ({longitude}, {latitude}): {elevation} meters");

            // 关闭数据集
            ds.Dispose();
        }

        static double GetElevationFromCoordinates(double longitude, double latitude, Dataset ds)
        {
            // 将经纬度转换为像素坐标
            int x, y;
            double[] geoTransform = new double[6];
            ds.GetGeoTransform(geoTransform);
            x = (int)((longitude - geoTransform[0]) / geoTransform[1]);
            y = (int)((latitude - geoTransform[3]) / geoTransform[5]);

            //var xGeo = geoTransform[0] + 107 * geoTransform[1] + 30 * geoTransform[2];
            //var yGeo = geoTransform[3] + 107 * geoTransform[4] + 30 * geoTransform[5];
            // 读取对应像素的高程值
            int[] bandNumbers = { 1 };
            double[] elevation = new double[1];
            ds.GetRasterBand(bandNumbers[0]).ReadRaster(x, y, 1, 1, elevation, 1, 1, 0, 0);

            return elevation[0];
        }
    }
}
