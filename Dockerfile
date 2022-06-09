ARG UNITY_VERSION=2021.2.12f1
ARG IMAGE_PLATFORM=base
ARG IMAGE_VERSION=1.0.1
ARG BASE_IMAGE=unityci/editor:ubuntu-$UNITY_VERSION-$IMAGE_PLATFORM-$IMAGE_VERSION

FROM $BASE_IMAGE

ARG BUILD_TARGET
ENV BUILD_TARGET $BUILD_TARGET

ARG UNITY_LICENSE
# ARG LICENSE_FILE_NAME

RUN echo "$UNITY_LICENSE" >> root/env-unity-license.ulf
# COPY $LICENSE_FILE_NAME root/$LICENSE_FILE_NAME

RUN unity-editor -batchmode -manualLicenseFile root/env-unity-license.ulf -logfile; exit 0
# RUN unity-editor -batchmode -manualLicenseFile root/$LICENSE_FILE_NAME -logfile; exit 0

WORKDIR /mnt
CMD unity-editor -batchmode -quit -nographics -projectPath $(pwd) -buildTarget $BUILD_TARGET -executeMethod UActions.Bootstrap.Run -job $JOB_NAME -logfile -
# | grep 'Build Finished'
