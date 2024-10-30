using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallRunBar : MonoBehaviour
{
        public Slider slider;

        public void setMaxStam(float max) {
            slider.maxValue = max;
            slider.value = max;
        }

        public void setCurStam(float cur) {
            slider.value = cur;
        }
    }
