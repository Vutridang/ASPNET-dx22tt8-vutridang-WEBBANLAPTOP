using System;

namespace WebBanLapTop
{
	/// <summary>
	/// Đại diện cho sản phẩm không đủ hàng trong giỏ
	/// </summary>
	[Serializable]
	public class InsufficientItem
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public int Stock { get; set; }
	}
}
