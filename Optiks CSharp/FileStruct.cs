using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Optiks_CSharp
{
    class FileStruct
    {
        public static byte[] HEADER = new byte[]
        {
            69,
            42,
            03,
            60,
            13,
            37
        };

        public static int STARTPOS = HEADER.Length;

        public static Scene toScene(byte[] bytes)
        {
            var q = from a in bytes.Take(STARTPOS)
                    join b in HEADER on a equals b
                    select a;

            if (q.Count() != HEADER.Length)
            {
                throw new IOException("Invalid header", 1);
            }

            try
            {
                double airN = BitConverter.ToDouble(bytes, STARTPOS);
                var bodies = new List<Body>();
                var lrays = new List<LightRay>();

                var index = STARTPOS + 8;

                while (bytes[index] != 0xff)
                {
                    var segs = new List<Line>();
                    
                    while (bytes[index] != 0xff)
                    {
                        if (bytes[index] == 0x00)
                        {
                            segs.Add(
                                new Segment(
                                    new Vector(
                                        BitConverter.ToDouble(bytes, index + 01),
                                        BitConverter.ToDouble(bytes, index + 09)
                                    ),
                                    new Vector(
                                        BitConverter.ToDouble(bytes, index + 17),
                                        BitConverter.ToDouble(bytes, index + 25)
                                    )
                                )
                            );
                            index += 33;
                        }
                        else
                        {
                            segs.Add(
                                new CircleArc(
                                    new Vector(
                                        BitConverter.ToDouble(bytes, index + 01),
                                        BitConverter.ToDouble(bytes, index + 09)
                                    ),
                                    new Vector(
                                        BitConverter.ToDouble(bytes, index + 17),
                                        BitConverter.ToDouble(bytes, index + 25)
                                    ),
                                    BitConverter.ToDouble(bytes, index + 33)
                                )
                            );
                            index += 41;
                        }
                    }
                    bodies.Add(
                        new Body(
                            segs,
                            BitConverter.ToDouble(bytes, index + 01),
                            (BodyTypes)bytes[index + 09],
                            new Pen(
                                Color.FromArgb(BitConverter.ToInt32(bytes, index + 10)),
                                BitConverter.ToSingle(bytes, index + 14)
                            ),
                            new SolidBrush(
                                Color.FromArgb(BitConverter.ToInt32(bytes, index + 18))
                            ),
                            (DrawTypes)bytes[index + 22]
                        )
                    );
                    index += 23;
                }
                index++;
                while (bytes[index] != 0xff)
                {
                    lrays.Add(
                        new LightRay(
                            new Ray(
                                new Vector(
                                    BitConverter.ToDouble(bytes, index + 00),
                                    BitConverter.ToDouble(bytes, index + 08)
                                ),
                                new Vector(
                                    BitConverter.ToDouble(bytes, index + 16),
                                    BitConverter.ToDouble(bytes, index + 24)
                                )
                            ),
                            BitConverter.ToInt32(bytes, index + 32),
                            new Pen(
                                Color.FromArgb(BitConverter.ToInt32(bytes, index + 36)),
                                BitConverter.ToSingle(bytes, index + 40)
                            )
                        )
                    );
                    index += 44;
                }

                return new Scene(bodies, lrays, airN);
            }
            catch
            {
                throw new IOException("Corrupted or invalid file", 2);
            }
        }

        public static byte[] toBytes(Scene scene)
        {
            var bytes = HEADER.ToList();

            bytes.AddRange(BitConverter.GetBytes(scene.airRefractionIndex));
            foreach (Body b in scene.bodies)
            {
                foreach (Line l in b.segments)
                {
                    bytes.Add((byte)l.type);

                    bytes.AddRange(BitConverter.GetBytes(l.start.x));
                    bytes.AddRange(BitConverter.GetBytes(l.start.y));
                    bytes.AddRange(BitConverter.GetBytes(l.end.x));
                    bytes.AddRange(BitConverter.GetBytes(l.end.y));

                    if (l.type == LineTypes.CircleArc)
                    {
                        bytes.AddRange(BitConverter.GetBytes(l.height * l.pointCW));
                    }
                }
                bytes.Add(0xff);

                bytes.AddRange(BitConverter.GetBytes(b.refractionIndex));
                bytes.Add((byte)b.type);
                bytes.AddRange(BitConverter.GetBytes(b.pen.Color.ToArgb()));
                bytes.AddRange(BitConverter.GetBytes(b.pen.Width));
                bytes.AddRange(BitConverter.GetBytes(b.brush.Color.ToArgb()));
                bytes.Add((byte)b.drawMode);
            }
            bytes.Add(0xff);

            foreach (LightRay r in scene.lightRays)
            {
                bytes.AddRange(BitConverter.GetBytes(r.rays[0].start.x));
                bytes.AddRange(BitConverter.GetBytes(r.rays[0].start.y));
                bytes.AddRange(BitConverter.GetBytes(r.rays[0].udir.x));
                bytes.AddRange(BitConverter.GetBytes(r.rays[0].udir.y));
                bytes.AddRange(BitConverter.GetBytes(r.maxRays));
                bytes.AddRange(BitConverter.GetBytes(r.pen.Color.ToArgb()));
                bytes.AddRange(BitConverter.GetBytes(r.pen.Width));
            }
            bytes.Add(0xff);

            return bytes.ToArray();
        }
    }
}