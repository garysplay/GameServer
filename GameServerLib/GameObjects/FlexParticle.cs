using GameServerCore.Domain.GameObjects;

namespace LeagueSandbox.GameServer.GameObjects
{
    //TODO: Investigate what exactly FlexParticles are and how they should get networked.
    public class FlexParticle : IFlexParticle
    {
        public IAttackableUnit Target { get; }
        public byte ParticleFlexID { get; }
        public byte CpIndex { get; }
        public uint ParticleAttachType { get; }

        public FlexParticle(IAttackableUnit target, byte particleFlexId, byte cpIndex, uint particleAttachType)
        {
            Target = target;
            ParticleFlexID = particleFlexId;
            CpIndex = cpIndex;
            ParticleAttachType = particleAttachType;
        }
    }


}
