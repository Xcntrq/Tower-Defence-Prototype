using UnityEngine;

namespace nsMousePositionHelper
{
    public class MousePositionHelper
    {
        private Camera _camera;

        public Vector3 MouseWorldPosition
        {
            get
            {
                Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0;
                return mouseWorldPosition;
            }
        }

        public MousePositionHelper(Camera camera)
        {
            _camera = camera;
        }
    }
}
