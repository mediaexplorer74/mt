﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIImageButton
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
  public class UIImageButton : UIElement
  {
    private float _visibilityActive = 1f;
    private float _visibilityInactive = 0.4f;
    private Texture2D _texture;

    public UIImageButton(Texture2D texture)
    {
      this._texture = texture;
      this.Width.Set((float) this._texture.get_Width(), 0.0f);
      this.Height.Set((float) this._texture.get_Height(), 0.0f);
    }

    public void SetImage(Texture2D texture)
    {
      this._texture = texture;
      this.Width.Set((float) this._texture.get_Width(), 0.0f);
      this.Height.Set((float) this._texture.get_Height(), 0.0f);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
      CalculatedStyle dimensions = this.GetDimensions();
      spriteBatch.Draw(this._texture, dimensions.Position(), Color.op_Multiply(Color.get_White(), this.IsMouseHovering ? this._visibilityActive : this._visibilityInactive));
    }

    public override void MouseOver(UIMouseEvent evt)
    {
      base.MouseOver(evt);
      Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
    }

    public void SetVisibility(float whenActive, float whenInactive)
    {
      this._visibilityActive = MathHelper.Clamp(whenActive, 0.0f, 1f);
      this._visibilityInactive = MathHelper.Clamp(whenInactive, 0.0f, 1f);
    }
  }
}
