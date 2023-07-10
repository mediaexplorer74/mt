using Microsoft.Xna.Framework;

namespace GameManager.Physics
{
	public interface IBallContactListener
	{
		void OnCollision(PhysicsProperties properties, Vector2 position, Vector2 velocity, BallCollisionEvent collision);

		void OnPassThrough(PhysicsProperties properties, Vector2 position, Vector2 velocity, float angularVelocity, BallPassThroughEvent passThrough);
	}
}
