using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace wtPay
{
    class ResourceManager
    {
        private static ResourceManager resourseManager = null;

        private Dictionary<string,Object> resourseDictionary = new Dictionary<string,Object>();

        private ResourceManager() { }

        public static ResourceManager getInstance()
        {
            if (resourseManager == null)
            {
                resourseManager = new ResourceManager();
            }
            return resourseManager;
        }
        /// <summary>
        /// 封装new操作，所有资源的new操作都调用此函数，有效防止内存泄漏
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// /// <param name="className">资源类名</param>
        /// <param name="resName">资源对象名称</param>
        public void addResource<T>(string resName ,string className)
        {
            T resObject = Util.ReflectionHelper.CreateInstance<T>(className);
            if (resObject == null)
            {
                throw new Exception("自定义异常！未找到类，确认类名是否全称");
            }
            this.resourseDictionary.Add(resName, resObject);
        }

        public object getResource(string resourceName)
        {

            if (this.resourseDictionary.ContainsKey(resourceName))
            {
                return this.resourseDictionary[resourceName];
            }
            Console.WriteLine("资源名称错误1");
            throw new Exception("自定义异常！是否资源名字错误或未初始化！");
        }
    }
}
