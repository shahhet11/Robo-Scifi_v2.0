using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer _beam;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private float _maxLength;

    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private GameObject _muzzleParticle;

    [SerializeField] private float _damage;
    // Start is called before the first frame update

    private void Awake()
    {
        _beam.enabled = false;
    }

    private void Activate()
    {
        _beam.enabled = true;
        _hitParticle.Play();
        _muzzleParticle.SetActive(true);
    }

    private void Deactivate()
    {
        _beam.enabled = false;
        _beam.SetPosition(0, _muzzlePoint.position);
        _beam.SetPosition(1, _muzzlePoint.position);

        _hitParticle.Stop();
        _muzzleParticle.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) Activate();
        else if (Input.GetMouseButtonUp(0)) Deactivate();
    }

    private void FixedUpdate()
    {
        if(!_beam.enabled) return;

        Ray ray = new Ray(_muzzlePoint.position,  Input.mousePosition);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, _maxLength);
        Debug.Log(cast+"CAST");
        Vector3 hitPosition = cast ? hit.point : _muzzlePoint.position + _muzzlePoint.forward * _maxLength;

        _beam.SetPosition(0, _muzzlePoint.position);
        RaycastHit hitinfo;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo, 10000f))
        {
            _beam.SetPosition(1, hitinfo.point);

            _hitParticle.transform.position = hitinfo.point;
            Vector3 direction = hitinfo.point - _muzzleParticle.transform.position;
            _muzzleParticle.transform.rotation = Quaternion.LookRotation(direction);
            //Debug.Log(hitinfo.transform.gameObject.name+"NAME");
            if (hitinfo.collider.TryGetComponent(out Damageable damageable))
            {
                
                damageable.ApplyDamage(_damage * Time.fixedDeltaTime);
            }
            else if (hitinfo.collider.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.UpdateHealth(2, hitinfo.point);
            }
        }

        
        

    }

}
