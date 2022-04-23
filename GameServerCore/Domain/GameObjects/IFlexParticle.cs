using GameServerCore.Domain.GameObjects;

public interface IFlexParticle
{
    IAttackableUnit Target { get; }
    byte ParticleFlexID { get; }
    byte CpIndex { get; }
    uint ParticleAttachType { get; }
}