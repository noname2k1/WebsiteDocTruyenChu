using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDocTruyenChu.Models
{
    public class ErrorResponse
    {
        public bool Success = false;
        public int StatusCode { get; set; }
        public string message { get; set; }

        public ErrorResponse(int StatusCode)
        {
            this.StatusCode = StatusCode;
            switch (StatusCode)
            {
                case 400:
                    this.message = "Yêu cầu không hợp lệ";
                    break;
                case 401:
                    this.message = "Xác thực không thành công";
                    break;
                case 403:
                    this.message = "Không có quyền truy cập tài nguyên";
                    break;
                case 404:
                    this.message = "Không tìm thấy tài nguyên yêu cầu";
                    break;
                default:
                    this.message = "Lỗi nội bộ máy chủ";
                    break;
            }
        }

        public ErrorResponse(int StatusCode, string Message)
        {
            this.StatusCode = StatusCode;
            this.message = Message;
        }
    }
}