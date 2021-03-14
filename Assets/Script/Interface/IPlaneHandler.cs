using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public interface IPlaneHandler
{
    IEnumerator RebornStart();

    void RebornEnd();

    IEnumerator InvincibleStart();

    void InvincibleEnd();
}