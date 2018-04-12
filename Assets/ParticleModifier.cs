using UnityEngine;
using System.Collections;
using UnityEditor;

public class ParticleModifier : MonoBehaviour {

    ParticleSystem particle;
    SerializedObject modifiableParticle;

    private float currentRadius;
	// Use this for initialization
	void Start () {
        particle = GetComponent<ParticleSystem>();

        modifiableParticle = new SerializedObject(particle);

        currentRadius = particle.shape.radius;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeRadius(0.5f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeRadius(-0.5f);
        }
	}

    void ChangeRadius(float delt)
    {
        currentRadius += delt;
        currentRadius = Mathf.Clamp(currentRadius, 0, 100);
        modifiableParticle.FindProperty("ShapeModule.radius").floatValue = currentRadius;   // 对于要控制的属性名字，可以将控制面板设为Debug模式
        print(modifiableParticle.FindProperty("ShapeModule.radius").floatValue);
        modifiableParticle.ApplyModifiedProperties();   // 别忘了这句，否则无效
    }
}
