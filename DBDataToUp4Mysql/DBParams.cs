using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBDataToUp4Mysql
{
    public class DBParams
    {
        private string plateno;
        private string clientid;
        private string timeweight;
        private string timeleave;
        private string allweight;
        private string weightleave;
        private string weightnet;
        private string sdic;
        private string type;
        private string plid;
        private string cdic;
        private string gdic;
        private string htid;
        private string qtyqr;
        private string sopr;
        private string sbid;
        private string scm;
        private string bdid;

        /// <summary>
        /// 车牌号
        /// </summary>
        public string Plateno { get => plateno; set => plateno = value; }
        /// <summary>
        /// 车辆运输单位
        /// </summary>
        public string Clientid { get => clientid; set => clientid = value; }
        /// <summary>
        /// 进场称重时间
        /// </summary>
        public string Timeweight { get => timeweight; set => timeweight = value; }
        /// <summary>
        /// 出厂称重时间
        /// </summary>
        public string Timeleave { get => timeleave; set => timeleave = value; }
        /// <summary>
        /// 毛重
        /// </summary>
        public string Allweight { get => allweight; set => allweight = value; }
        /// <summary>
        /// 皮重
        /// </summary>
        public string Weightleave { get => weightleave; set => weightleave = value; }
        /// <summary>
        /// 净重
        /// </summary>
        public string Weightnet { get => weightnet; set => weightnet = value; }
        /// <summary>
        /// 库位
        /// </summary>
        public string Sdic { get => sdic; set => sdic = value; }
        /// <summary>
        /// 称重类型  0:采购入库;1:销售出库
        /// </summary>
        public string Type { get => type; set => type = value; }
        /// <summary>
        /// 计划编号
        /// </summary>
        public string Plid { get => plid; set => plid = value; }
        /// <summary>
        /// 客户信息&供应商信息
        /// </summary>
        public string Cdic { get => cdic; set => cdic = value; }
        /// <summary>
        /// 货品信息
        /// </summary>
        public string Gdic { get => gdic; set => gdic = value; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string Htid { get => htid; set => htid = value; }
        /// <summary>
        /// 对方数量
        /// </summary>
        public string Qtyqr { get => qtyqr; set => qtyqr = value; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string Sopr { get => sopr; set => sopr = value; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string Sbid { get => sbid; set => sbid = value; }
        /// <summary>
        /// 公司编码
        /// </summary>
        public string Scm { get => scm; set => scm = value; }
        /// <summary>
        /// 榜单号
        /// </summary>
        public string Bdid { get => bdid; set => bdid = value; }



    }
}
