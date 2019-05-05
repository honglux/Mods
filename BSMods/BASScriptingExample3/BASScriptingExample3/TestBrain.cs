namespace BASScriptingExample3
{
    using BS;
    using UnityEngine;

    public sealed class TestBrain : BrainHuman
    {
        private bool shouldLeaveNow;

        public TestBrain()
        {
        }

        // Override OnUpdateCycle() to do nothing but wait for damage, then leave.
        public override void OnUpdateCycle()
        {
            if (this.shouldLeaveNow)
            {
                if (!this.canLeave)
                {
                    return;
                }

                this.TryLeave(1f);
            }
        }

        public override void OnDamage(float damage, float finalDamage, Damager.DamageType type, float recoil, float knockOutDuration, RagdollPart bodyPart, Vector3 velocity, Damager damager)
        {
            base.OnDamage(damage, finalDamage, type, recoil, knockOutDuration, bodyPart, velocity, damager);
            this.shouldLeaveNow = true;
        }
    }
}