ARG UNITY_VERSION=2021.2.12f1
ARG IMAGE_PLATFORM=base
ARG IMAGE_VERSION=1.0.1
ARG BASE_IMAGE=unityci/editor:ubuntu-$UNITY_VERSION-$IMAGE_PLATFORM-$IMAGE_VERSION

FROM $BASE_IMAGE

ARG BUILD_TARGET
ARG UNITY_LICENSE

ENV BUILD_TARGET $BUILD_TARGET

RUN echo $UNITY_LICENSE >> /root/unity-license.ulf

RUN unity-editor -batchmode -manualLicenseFile /root/unity-license.ulf -logfile; exit 0

WORKDIR /mnt
CMD unity-editor -batchmode -quit -nographics -projectPath $(pwd) -buildTarget $BUILD_TARGET -executeMethod UActions.Bootstrap.Run -job $JOB_NAME -logfile -
# | grep 'Build Finished'
