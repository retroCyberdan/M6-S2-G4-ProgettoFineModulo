using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour
{
    // PREMESSA:
    //
    // Poichè ho speso molto tempo nel progettarlo, ho evitato di utilizzare il Cinemachine (che comunque ho implementato nel progetto)
    // ed ho preferito utilizza questo script per la rotazione della camera tramite mouse.
    //
    // Spero venga apprezzato :)
    //
    [SerializeField] private Transform _target;           // <- soggetto attorno a cui ruotare
    [SerializeField] private float _distance = 5.0f;      // <- distanza dal target
    [SerializeField] private float _xSpeed = 120.0f;      // <- velocità di rotazione orizzontale
    [SerializeField] private float _ySpeed = 80.0f;       // <- velocità di rotazione verticale

    [SerializeField] private float _yMinLimit = -20f;     // <- limite minimo angolo verticale
    [SerializeField] private float _yMaxLimit = 80f;      // <- limite massimo angolo verticale

    // origine
    private float _x = 0.0f;
    private float _y = 0.0f;

    void Start()
    {
        Debug.Log($"Benvenuto! Raccogli le 3 schede di sicurezza sparse per lo scenario e portale al globo rosso per vincere!");

        Vector3 angles = transform.eulerAngles;
        _x = angles.y;
        _y = angles.x;

        // blocca il cursore (opzionale poichè ho notato che, se attivo, non funzionano le UI di sconfitta/vittoria)
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (_target)
        {
            _x += Input.GetAxis("Mouse X") * _xSpeed * Time.deltaTime;
            _y -= Input.GetAxis("Mouse Y") * _ySpeed * Time.deltaTime;

            _y = Mathf.Clamp(_y, _yMinLimit, _yMaxLimit);

            Quaternion rotation = Quaternion.Euler(_y, _x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -_distance) + _target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
}
