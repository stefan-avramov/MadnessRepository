﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaGooseGame
{
	interface IInteractiveObject
	{
		void Interact(PlayerElement player);
		bool CanInteract(PlayerElement player);
	}
}
