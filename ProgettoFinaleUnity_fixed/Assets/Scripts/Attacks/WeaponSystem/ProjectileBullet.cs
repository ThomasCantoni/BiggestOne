using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBullet : GenericBullet
{
    public ProjectileBulletMonobehaviour ProjectileBulletGameObject;
    
    public Transform PositionRotation;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="GameObjectToInstantiate">
    /// ProjectileBullet cannot exist on it's own, as it is not a monobehaviour child. 
    /// The GameObject parameter is the GameObject that functions as the physical extension of ProjectileBullet in order to act on the world properly.
    /// </param>
    /// <param name="PositionRotation">This transform represents the position and rotation the bullet should have.</param>
    ///
    public ProjectileBullet(GenericGun owner,ProjectileBulletMonobehaviour GameObjectToInstantiate, Transform PositionRotation) : base(owner)
    {
        this.ProjectileBulletGameObject = GameObjectToInstantiate;
        this.PositionRotation = PositionRotation;
       // ProjectileBulletGameObject.ProjectileBullet = this;
    }
    public override void Deploy()
    {
        ProjectileBulletGameObject=   GameObject.Instantiate(ProjectileBulletGameObject, PositionRotation.position,PositionRotation.rotation);
        ProjectileBulletGameObject.ProjectileBullet = this;
        ProjectileBulletGameObject.Source = this.Owner;
    }
}
