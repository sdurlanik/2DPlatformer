using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingBehaviour : StateMachineBehaviour
{
    private ParticleSystem _jumpParticle;
    private ParticleSystem.VelocityOverLifetimeModule _velocityModule;
    private ParticleSystem.ShapeModule _shapeModule;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _jumpParticle = animator.transform.GetChild(0).GetComponent<ParticleSystem>();
        _velocityModule = _jumpParticle.velocityOverLifetime;
        _shapeModule = _jumpParticle.shape;

        if (animator.GetBool("isGrounded") && !animator.GetBool("isFalling")) 
        {
            _velocityModule.x = 0;
            _velocityModule.y = 1.2f;
            _shapeModule.scale = new Vector3(0.8f, 0, 0);
            _shapeModule.position = new Vector3(0, -.1f, 0);
            _jumpParticle.Play();
        }
       
        
        //Scale x 0.8 pos y -.1
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
       
   // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
      
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
