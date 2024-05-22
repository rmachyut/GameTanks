/*
*
*   Anything that is going to be an interactable object in your game should extend from GameObject.  
*       It handles the life-cycle of the objects, some useful general features (such as tags), and serves 
*       as the convenient facade to making the object work with the physics system.  It's a good class, Bront.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Collections.Generic;

namespace Shard
{
    class GameObject
    {
        private Transform3D transform;
        private bool bTransient;
        private bool bDestroy;
        private bool visible;
        private PhysicsBody physicsBody;
        private List<string> tags;

        public void addTag(string str)
        {
            if (tags.Contains(str))
            {
                return;
            }

            tags.Add(str);
        }

        public void removeTag(string str)
        {
            tags.Remove(str);
        }

        public bool checkTag(string tag)
        {
            return tags.Contains(tag);
        }

        public String getTags()
        {
            string str = "";

            foreach (string s in tags)
            {
                str += s;
                str += ";";
            }

            return str;
        }

        public void setPhysicsEnabled()
        {
            PhyBody = new PhysicsBody(this);
        }


        public bool queryPhysicsEnabled()
        {
            if (PhyBody == null)
            {
                return false;
            }
            return true;
        }

        internal Transform3D Transform
        {
            get => transform;
        }

        internal Transform Transform2D
        {
            get => (Transform)transform;
        }


        public bool Visible
        {
            get => visible;
            set => visible = value;
        }
        public bool Transient { get => bTransient; set => bTransient = value; }
        public bool ToBeDestroyed { get => bDestroy; set => bDestroy = value; }
        internal PhysicsBody PhyBody { get => physicsBody; set => physicsBody = value; }

        public virtual void initialize()
        {
        }

        public virtual void update()
        {

        }

        public virtual void physicsUpdate()
        {
        }

        public virtual void prePhysicsUpdate()
        {
        }

        public GameObject()
        {
            GameObjectManager.getInstance().addGameObject(this);

            transform = new Transform3D(this);
            visible = false;

            ToBeDestroyed = false;
            tags = new List<string>();

            this.initialize();

        }

        public void checkDestroyMe()
        {

            if (!bTransient)
            {
                return;
            }

            if (Transform.X > 0 && Transform.X < Bootstrap.getDisplay().getWidth())
            {
                if (Transform.Y > 0 && Transform.Y < Bootstrap.getDisplay().getHeight())
                {
                    return;
                }
            }


            ToBeDestroyed = true;

        }

        public virtual void killMe()
        {
            PhysicsManager.getInstance().removePhysicsObject(physicsBody);

            physicsBody = null;
            transform = null;
        }


    }
}
