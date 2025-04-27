using System;
using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.GameStructs;
using RogueCastle.Screens.BaseScreens;

namespace RogueCastle;

public class ProjectileIconPool(int poolSize, ProjectileManager pm, RCScreenManager rcs) : IDisposable {
    private ProjectileManager _projectileManager = pm;
    private DS2DPool<ProjectileIconObj> _resourcePool = new();
    private RCScreenManager _screenManager = rcs;

    public bool IsDisposed { get; private set; }

    public int ActiveTextObjs => _resourcePool.NumActiveObjs;

    public int TotalPoolSize => _resourcePool.TotalPoolSize;

    public int CurrentPoolSize => TotalPoolSize - ActiveTextObjs;

    public void Initialize() {
        for (var i = 0; i < poolSize; i++) {
            var poolObj = new ProjectileIconObj {
                Visible = false,
                ForceDraw = true,
                TextureColor = Color.White,
            };

            _resourcePool.AddToPool(poolObj);
        }
    }

    public void AddIcon(ProjectileObj proj) {
        var icon = _resourcePool.CheckOut();
        icon.Visible = true;
        icon.ForceDraw = true;
        icon.AttachedProjectile = proj; // Linking the projectile and the icon.
        proj.AttachedIcon = icon;
    }

    public void DestroyIcon(ProjectileObj proj) {
        var icon = proj.AttachedIcon;
        icon.Visible = false;
        icon.Rotation = 0;
        icon.TextureColor = Color.White;
        icon.Opacity = 1;
        icon.Flip = SpriteEffects.None;
        icon.Scale = new Vector2(1, 1);
        _resourcePool.CheckIn(icon);

        icon.AttachedProjectile = null; // De-linking the projectile and the icon.
        proj.AttachedIcon = null;
    }

    public void DestroyAllIcons() {
        foreach (var proj in _projectileManager.ActiveProjectileList.Where(proj => proj.AttachedIcon != null))
        {
            DestroyIcon(proj);
        }
    }

    public void Update(Camera2D camera) {
        var player = _screenManager.Player;
        foreach (var proj in _projectileManager.ActiveProjectileList.Where(proj => proj.ShowIcon)) {
            if (proj.AttachedIcon is not null) {
                // Destroy projectile icons if they get in camera range.
                if (CollisionMath.Intersects(proj.Bounds, camera.Bounds)) {
                    DestroyIcon(proj);
                }

                return;
            }

            if (CollisionMath.Intersects(proj.Bounds, camera.Bounds)) {
                continue;
            }

            // Using 1 because it needs a margin of error.
            if ((proj.AccelerationX > 1 && proj.X < player.X && proj.Y > camera.Bounds.Top && proj.Y < camera.Bounds.Bottom) ||
                (proj.AccelerationX < -1 && proj.X > player.X && proj.Y > camera.Bounds.Top && proj.Y < camera.Bounds.Bottom) ||
                (proj.AccelerationY > 1 && proj.Y < player.Y && proj.X > camera.Bounds.Left && proj.X < camera.Bounds.Right) ||
                (proj.AccelerationY < -1 && proj.Y > player.Y && proj.X > camera.Bounds.Left && proj.X < camera.Bounds.Right)) {
                AddIcon(proj);
            }
        }

        // A check to make sure projectiles that die do not have a lingering icon attached to them.
        for (var i = 0; i < _resourcePool.ActiveObjsList.Count; i++) {
            if (_resourcePool.ActiveObjsList[i].AttachedProjectile.IsAlive) {
                continue;
            }

            DestroyIcon(_resourcePool.ActiveObjsList[i].AttachedProjectile);
            i--;
        }

        foreach (var projIcon in _resourcePool.ActiveObjsList) {
            projIcon.Update(camera);
        }
    }

    public void Draw(Camera2D camera) {
        if (Game.PlayerStats.HasTrait(TraitType.TUNNEL_VISION)) {
            return;
        }

        foreach (var projIcon in _resourcePool.ActiveObjsList) {
            projIcon.Draw(camera);
        }
    }

    public void Dispose() {
        if (IsDisposed) {
            return;
        }

        Console.WriteLine(@"Disposing Projectile Icon Pool...");
        IsDisposed = true;

        _resourcePool.Dispose();
        _resourcePool = null;
        _projectileManager = null;
        _screenManager = null;
    }
}
