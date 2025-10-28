using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // ����: ����Collider���Փ˂�������́AUnity�́uProject Settings�v->�uPhysics�v��
    //       �uLayer Collision Matrix�v�Őݒ肳��Ă��܂��B
    //       ��: �uEnemyAttack�v���C���[�́uPlayerHitbox�v���C���[�̂ݏՓ˂�������Ă���K�v������܂��B

    private StatusManager receiverStatus; // �_���[�W���󂯂鑤��StatusManager

    void Start()
    {
        // ���g�̐e�I�u�W�F�N�g����StatusManager���擾
        receiverStatus = GetComponentInParent<StatusManager>();

        if (receiverStatus == null)
        {
            Debug.LogError("Hitbox�̐e��StatusManager��������܂���B");
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �Փ˔���ɂ��āF
        // �����G���W�����x���ł̎��O�t�B���^�����O�́A���C���[�ݒ�ōs���Ă��܂��B(Enemy���m���Ԃ���Ȃ��悤�ɁA��)
        // Hitbox��Attack�́A�G�Ί֌W�̂��鑊��ɂ̂ݓ����蔻�肪�����Ԃł��̃X�N���v�g�͋@�\���܂��B
        // �����Ӑ}���Ȃ��Փ˔��肪�N�����ꍇ�͍ŏ��Ƀ��C���[�}�X�N���m�F���Ă��������B

        // ��e����StatusManager�ɒʒm
        receiverStatus.Damage();

    }
}
