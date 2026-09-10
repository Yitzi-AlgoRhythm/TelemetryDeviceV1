using Confluent.Kafka;
using ParameterDataLib;
using TelemetryDeviceV1.ICD;
using TelemetryDeviceV1.ICD.Enums;

namespace TelemetryDeviceV1.Dataflow.Stages.Helpers
{
    public class PacketData
    {
        private const int correlatorBytePosition = 8;
        private const int dataStartByte = 9;


        private byte[] _data;

        private ulong _timestampMS;

        public byte CorrelatorByte { get; private set; }

        public PacketData(IEnumerable<byte> data)
        {
            byte[] dataArr = data.ToArray();

            _data = dataArr[dataStartByte..];

            _timestampMS = BitConverter.ToUInt64(dataArr);

            CorrelatorByte = dataArr[correlatorBytePosition];
        }

        public ParameterData GetParameterData(IcdParameter icdParam)
        {
            byte[] valueArr = _data.Skip(icdParam.Offset).Take(icdParam.Size).ToArray();

            ValueBase value;

            if (icdParam.Type == ParameterDataType.Float64)
            {
                value = new DoubleValue(BitMaskService.GetFloat64(valueArr, icdParam.BitMask));
            }
            else
            {
                value = new IntValue(BitMaskService.GetInt32(valueArr, icdParam.BitMask, icdParam.Type));
            }

            return new ParameterData()
            {
                Name = icdParam.Name,
                Type = icdParam.Type == ParameterDataType.Float64 ? DataType.Double : DataType.Int,
                Units = icdParam.Unit,
                Value = value,
                TimestampMS = _timestampMS
            };
        }
    }
}
