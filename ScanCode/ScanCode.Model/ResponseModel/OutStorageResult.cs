using ScanCode.Model.Entity;
using System;
using System.Collections.Generic;

namespace ScanCode.Model.ResponseModel;

public class OutStorageResult : TResult
{
    public OutStorageResult()
    {
        LabelStorages = new List<W_LabelStorage>(1000);
    }

    public bool Successed { get; set; }

    public DateTime Time { get; set; }

    public int Count { get; set; }

    public string QRCode { get; set; }

    public List<W_LabelStorage> LabelStorages { get; set; }

    public string Message { get; set; }

    public static OutStorageResult Success(DateTime time, int count, string qrCode)
    {
        return new OutStorageResult()
        {
            Successed = true,
            Time = time,
            Count = count,
            QRCode = qrCode
        };
    }

    public static OutStorageResult Fail(string message, string qrCode)
    {
        return new OutStorageResult()
        {
            Successed = false,
            Message = message,
            QRCode = qrCode
        };
    }

    public static OutStorageResult FailList(string message, List<W_LabelStorage> labelStorages)
    {
        return new OutStorageResult()
        {
            Successed = false,
            Message = message,//string.Join(",", messageList),
            LabelStorages = labelStorages
        };
    }
}