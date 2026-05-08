using System.Buffers.Binary;
using System.Text;

namespace rasdaq.Audio;

internal class WAVLoader
{
    /// <summary>
    /// Format information about the WAV file.
    /// </summary>
    public struct FmtChunk
    {
        public ushort AudioFormat;
        public ushort NumChannels;
        public uint SampleRate;
        public uint ByteRate;
        public ushort BlockAlign;
        public ushort BitsPerSample;
        public FmtChunkExtension? Extension;
    };
    /// <summary>
    /// Extra format information for certain WAV files.
    /// </summary>
    public struct FmtChunkExtension
    {
        public ushort ValidBitsPerSample;
        public uint SpeakerPositionMask;
        public Guid SubFormat;
    }

    /// <summary>
    /// Holds the data of the WAV file, should be PCM data, as that is all is supported currently.
    /// </summary>
    public struct DataChunk
    {
        public uint Size;
        public byte[] Data;
    }

    /// <summary>
    /// Holds all the information necessary about a WAV file, namely,
    /// the format information and the data itself.
    /// </summary>
    public struct WAVData
    {
        public FmtChunk Format;
        public DataChunk Data;
    }

    private static byte[] ReadBytesFromStream(FileStream stream, int count)
    {
        byte[] buffer = new byte[count];
        stream.ReadExactly(buffer, 0, count);
        return buffer;
    }

    private static FmtChunk ProcessFmtChunck(FileStream stream)
    {
        uint size = BinaryPrimitives.ReadUInt32LittleEndian(ReadBytesFromStream(stream, 4));

        FmtChunk chunk = new FmtChunk();
        chunk.AudioFormat = BinaryPrimitives.ReadUInt16LittleEndian(ReadBytesFromStream(stream, 2));
        chunk.NumChannels = BinaryPrimitives.ReadUInt16LittleEndian(ReadBytesFromStream(stream, 2));
        chunk.SampleRate = BinaryPrimitives.ReadUInt32LittleEndian(ReadBytesFromStream(stream, 4));
        chunk.ByteRate = BinaryPrimitives.ReadUInt32LittleEndian(ReadBytesFromStream(stream, 4));
        chunk.BlockAlign = BinaryPrimitives.ReadUInt16LittleEndian(ReadBytesFromStream(stream, 2));
        chunk.BitsPerSample = BinaryPrimitives.ReadUInt16LittleEndian(ReadBytesFromStream(stream, 2));

        if (size > 16)
        {
            ushort extensionSize = BinaryPrimitives.ReadUInt16LittleEndian(ReadBytesFromStream(stream, 2));
            if (extensionSize == 22)
            {
                chunk.Extension = new FmtChunkExtension()
                {
                    ValidBitsPerSample = BinaryPrimitives.ReadUInt16LittleEndian(ReadBytesFromStream(stream, 2)),
                    SpeakerPositionMask = BinaryPrimitives.ReadUInt32LittleEndian(ReadBytesFromStream(stream, 4)),
                    SubFormat = new Guid(ReadBytesFromStream(stream, 16))
                };
            }
        }

        return chunk;
    }

    private static DataChunk ProcessDataChunk(FileStream stream)
    {
        DataChunk chunk = new DataChunk();
        chunk.Size = BinaryPrimitives.ReadUInt32LittleEndian(ReadBytesFromStream(stream, 4));
        chunk.Data = ReadBytesFromStream(stream, (int)chunk.Size);
        return chunk;
    }

    /// <summary>
    /// Load the WAV file at the given path and return a WAVData struct containing the format information and the data itself.
    /// </summary>
    /// <param name="path">The path to the WAV file.</param>
    /// <returns>A WAVData struct containing the format information and the data.</returns>
    /// <exception cref="Exception">Thrown if the WAV file is not found or is invalid.</exception>
    public static WAVData LoadWav(string path)
    {
        if (!File.Exists(path))
        {
            throw new Exception("WAV file not found: " + path);
        }

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            // Read the RIFF header at the start of the WAV file.
            byte[] fourcc = new byte[4];
            stream.ReadExactly(fourcc, 0, 4);
            if (Encoding.ASCII.GetString(fourcc) != "RIFF")
            {
                throw new Exception("Invalid WAV file: missing RIFF header.");
            }

            // Read the file size
            byte[] byteFileSize = new byte[4];
            stream.ReadExactly(byteFileSize, 0, 4);
            int fileSize = BinaryPrimitives.ReadInt32LittleEndian(byteFileSize) - 4;

            // Check file type is WAVE
            byte[] byteFileType = new byte[4];
            stream.ReadExactly(byteFileType, 0, 4);
            Encoding.UTF8.GetString(byteFileType);
            if (Encoding.ASCII.GetString(byteFileType) != "WAVE")
            {
                throw new Exception("Invalid WAV file: file type not listed as WAVE in RIFF header.");
            }

            FmtChunk fmtChunk = new FmtChunk();
            DataChunk dataChunk = new DataChunk();

            // Now read the remaining chunks, we want a fmt chunk and a data chunk.
            while (stream.Position < stream.Length - 4)
            {
                byte[] chunkId = new byte[4];
                stream.ReadExactly(chunkId, 0, 4);

                if (chunkId.SequenceEqual(Encoding.ASCII.GetBytes("fmt ")))
                {
                    fmtChunk = ProcessFmtChunck(stream);

                    if (fmtChunk.AudioFormat != 1)
                    {
                        throw new Exception("Unsupported WAV file: only PCM format is supported.");
                    }
                }
                else if (chunkId.SequenceEqual(Encoding.ASCII.GetBytes("data")))
                {
                    dataChunk = ProcessDataChunk(stream);
                    break;
                }
                else
                {
                    throw new Exception("Unsupported WAV file: unknown chunk type " + Encoding.ASCII.GetString(chunkId));
                }
            }

            return new WAVData()
            {
                Format = fmtChunk,
                Data = dataChunk
            };
        }
    }
}
