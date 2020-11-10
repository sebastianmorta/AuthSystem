using System;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
