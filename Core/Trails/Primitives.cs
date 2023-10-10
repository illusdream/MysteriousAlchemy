using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Core.Trails
{
    public class Primitives : IDisposable
    {
        public bool IsDisposed { get; private set; }

        //暂存于显存中的顶点数据
        private DynamicVertexBuffer vertexBuffer;
        private DynamicIndexBuffer indexBuffer;

        private readonly GraphicsDevice graphicsDevice;

        public Primitives(GraphicsDevice device, int maxVertices, int maxIndices)
        {
            this.graphicsDevice = device;
            if (device != null)
            {
                Main.QueueMainThreadAction(() =>
                {
                    vertexBuffer = new DynamicVertexBuffer(device, typeof(VertexPositionColorTexture), maxVertices, BufferUsage.None);
                    indexBuffer = new DynamicIndexBuffer(device, IndexElementSize.SixteenBits, maxIndices, BufferUsage.None);
                });
            }
        }
        public void Render(Action effectAction)
        {
            if (vertexBuffer is null || indexBuffer is null)
                return;

            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;

            effectAction?.Invoke();
            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
        }
        public void SetVertices(VertexPositionColorTexture[] vertices)
        {
            //SetDataOptions.Discard 这个的作用是表示不再需要之前的顶点信息，将在显存的一个新的位置存储数据，
            //此时显卡还仍然可以使用老数据，一旦写入过程完成，显卡将使用新数据而抛弃老数据
            vertexBuffer?.SetData(0, vertices, 0, vertices.Length, VertexPositionColorTexture.VertexDeclaration.VertexStride, SetDataOptions.Discard);
        }

        public void SetIndices(short[] indices)
        {
            indexBuffer?.SetData(0, indices, 0, indices.Length, SetDataOptions.Discard);
        }
        public void Dispose()
        {
            IsDisposed = true;

            vertexBuffer?.Dispose();
            indexBuffer?.Dispose();
        }
    }
}
