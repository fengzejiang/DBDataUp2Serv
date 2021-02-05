using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBDataToUp4Access
{
    public class DBParams
    {
        private string plateno;//车牌号
        private string clientid;
        private string timeweight;//进场时间
        private string timeleave;//离场时间
        private string allweight;//毛重
        private string weightleave;//皮重
        private string weightnet;//净重
        private string sdic;//仓库
        private string type;
        private string plid;
        private string cdic;//供应商信息
        private string gdic;//物料名称
        private string htid;
        private string qtyqr;
        private string sopr;//业务员
        private string sbid;//设备号
        private string scm;//公司编码
        private string bdid;//磅单编号

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
